if (top.HlpSys === undefined) {
    top.HlpSys = new Object();
}

top.HlpSys.search = function(){
    var self = null,
        workingDocument = null,
        platformType = (function() {
            var userAgent = window.navigator.userAgent;

            if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i)) {
              // iPad or iPhone
              return 'mobile';
            } else {
              // Anything else
              return 'desktop';
            }
        })();


    var searchState = new CookieWorker(top.document, "autodesk_search_state", 24000);


    function saveState(config) {
        searchState.store(config);
    }

    function restoreState(config) {
        var hideRanking = config.hideRanking;
        //load config from cookies or localStorage
        searchState.load(config);
        config.hideRanking = hideRanking;
        //convert string values into boolean
        config.highlightEnable = (!(config.highlightEnable === "false" || config.highlightEnable === false));
        config.wholeWords = (config.wholeWords === "true" || config.wholeWords === true);
        config.caseSensitive = (config.caseSensitive === "true" || config.caseSensitive === true);
    }

    function prepareQuery(query) {
        //replace stop symbols
        query = query.replace(/\\/g, "\\\\");
        query = query.replace(/[\[\^\$\.\|\+\(\)`~!#%&\-=\]{};'"<>,]/g, " ");
        query = query.replace(/\*/g, "[^\\s]*");
        query = query.replace(/\?/g, ".");

        //remove white space
        var regexp = /(\x20\x20)/g;
        while (query.match(regexp)) query = query.replace(regexp, "\x20");
        regexp = /(^\x20)|(\x20$)/g;
        while (query.match(regexp)) query = query.replace(regexp, "");

        return query;
    };

    function filterQuery(queryArr, buffer, data) {
        var isStopWord = false;
        var isSameWord = false;
        var resultArr = new Array();
        var filteredStopWords = "";
        var StopWords = data.stopWordsList;

        for (var i = 0; i < queryArr.length; i++) {
            isStopWord = false;
            isSameWord = false;
            if (StopWords[queryArr[i].toLowerCase()]) {
                isStopWord = true;
            }
            for (var j = 0; j < i; j++) {
                if (queryArr[j] == queryArr[i]) {
                    isSameWord = true;
                    break;
                }
            }
            if (!isStopWord && !isSameWord) {
                resultArr[resultArr.length] = queryArr[i];
            } else {
                filteredStopWords += "\"" + queryArr[i] + "\" ";
            }
        }

        buffer.filteredStopWords = filteredStopWords;
        buffer.querryWordList = resultArr;
        return resultArr;
    };

    function generateRegExpArray(queryArr, stemmedArr, config) {
        var resultArray = new Array(queryArr.length);
        var caseSensitive = config.caseSensitive;
        var wholeWords = config.wholeWords;
        var parameters = "";
        for (var i = 0; i<queryArr.length; i++) {
            if (caseSensitive) {
                parameters = "";
            } else {
                parameters = "i";
            }
            if (wholeWords)
                resultArray[i] = new RegExp('\\b' + '('+queryArr[i]+')|('+stemmedArr[i]+')' + '\\b', parameters);
            else
                resultArray[i] = new RegExp('('+queryArr[i]+')|('+stemmedArr[i]+')', parameters);
        }

        return resultArray;
    };

    function RenderBookList(searchProviders) {
        if (workingDocument !== null) {
            var bookList = workingDocument.getElementById("collapsible_search_books");

            if (bookList) {
                for (var i in searchProviders) {
                    var searchProvider = searchProviders[i];
                    var bookElement = workingDocument.createElement("div");
                    var bookSwitch = workingDocument.createElement("input");
                    bookSwitch.setAttribute("id", "book" + searchProvider.getName());
                    bookSwitch.setAttribute("type", "checkbox");
                    if (searchProvider.isEnabled()) {
                        bookSwitch.setAttribute("checked", "true");
                    }
                    bookSwitch.setAttribute("onClick", "top.HlpSys.search.data.searchProviders['"+i+"'].switchState()");
                    bookElement.appendChild(bookSwitch);

                    var text = workingDocument.createTextNode(searchProvider.getFullName());
                    bookElement.appendChild(text);
                    bookList.appendChild(bookElement);
                }
            }
        }
    };

    function setupResultList(platformType, doc) {
        if (platformType === 'mobile') {
            // Replace <select> element with <ul> to show list properly on iOS devices.
            var searchList = doc.getElementById('searchList'),
                newSearchList = document.createElement('ul'),
                wrapper = document.createElement('div')
                self = this;

            newSearchList.setAttribute('class', 'searchResultList');
            newSearchList.setAttribute('id', 'searchList');
            newSearchList.setAttribute('onclick', 'top.HlpSys.search.ui.viewResult(document, event);');

            wrapper.setAttribute('class', 'searchResultListWrapper');
            wrapper.appendChild(newSearchList);

            searchList.parentNode.replaceChild(wrapper, searchList);
        } else {
            // Leave <select> as is for desktop browsers.
        }
    };

    function displaySearchResultDesktop(resultList, resultsDocument, config) {
        var results = resultList.getResults(),
            searchQuery = resultList.getSearchQuery(),
            searchList = resultsDocument.getElementById("searchList");

        searchList.length = 0;

        for (var i = 0; i < results.length; i++) {
            var element = resultsDocument.createElement("OPTION");

            if (config.hideRanking) {
                element.text = results[i].title;
            } else {
                element.text = results[i].rank + ": " + results[i].title;
            }

            element.value = results[i].href + "?" + searchQuery;

            try {
                searchList.add(element, null);
                // standards compliant
            } catch(ex) {
                searchList.add(element);
                // IE only
            }
        }

        if (searchList.length == 0) {
            this.showWarning("no.results");
        }
    };

    function displaySearchResultMobile(resultList, resultsDocument, config) {
        var results = resultList.getResults(),
            searchQuery = resultList.getSearchQuery(),
            searchList = resultsDocument.getElementById("searchList"),
            label;

        // Clear list.
        while(searchList.hasChildNodes()){
            searchList.removeChild(searchList.childNodes[0]);
        }

        // Populate list with search result items.
        var li, span;
        for (var i = 0; i < results.length; i++) {
            li = resultsDocument.createElement("li");
            li.setAttribute('data-href', results[i].href + "?" + searchQuery);
            span = resultsDocument.createElement("span");

            if (config.hideRanking) {
                span.innerHTML = results[i].title;
            } else {
                span.innerHTML = results[i].rank + ": " + results[i].title;
            }

            li.appendChild(span);

            searchList.appendChild(li);
        }

        if (results.length == 0) {
            this.showWarning("no.results");
        }
    };

    function viewResultDesktop(resultsDocument) {
        var searchList = resultsDocument.getElementById("searchList");

        if (searchList.selectedIndex > -1 ) {
            window.open(searchList.options[searchList.selectedIndex].value, "content");
        }
    };

    function viewResultMobile(resultsDocument, event) {
        var li;
        if (event.target.nodeName === 'SPAN') {
           li = event.target.parentNode;
        } else {
           li = event.target;
        }

        var selectedHref = li.getAttribute('data-href');
        if (selectedHref) {
            window.open(selectedHref, "content");
        }
    };

    return {
        init : function(doc) {
            self = this;
            workingDocument = doc;
            restoreState(this.config);
            setupResultList(platformType, workingDocument);
            RenderBookList(this.data.searchProviders);
        },

        restoreState: function(doc) {
            var searchForm = doc.searchForm;

            var searchMethodState = this.config.searchMethod;
            for (var i = 0; i < searchForm.searchMethod.length; i++) {
                var radioElement = searchForm.searchMethod[i];
                if (radioElement.value == searchMethodState) {
                    radioElement.checked = true;
                }
            }

            var highlightCheckBox = searchForm.highlightSwitch;
            if (highlightCheckBox) {
                var highlightState = this.config.highlightEnable;
                highlightCheckBox.checked = highlightState;
            }

            var caseSensitiveSwitch = searchForm.caseSensitiveSwitch;
            if (caseSensitiveSwitch) {
                var caseSensitiveState = this.config.caseSensitive;
                caseSensitiveSwitch.checked = caseSensitiveState;
            }

            var wholeWordsSwitch = searchForm.wholeWordsSwitch;
            if (wholeWordsSwitch) {
                var wholeWordsState = this.config.wholeWords;
                wholeWordsSwitch.checked = wholeWordsState;
            }

            var searchTermField = searchForm.searchData;
            if (searchTermField) {
                var searchTermState = this.config.searchTerm;
                searchTermField.value = searchTermState;
            }
        },

        config: {
            workingDocument : null,
			caseSensitive: false,
			wholeWords: false,
			searchMethod: 'and',
			highlightEnable: true,
            searchTerm: "",
            hideRanking: false,

            setSearchMethod : function(option) {
                this.searchMethod = option;
                saveState(this);
            },

            setWholeWords : function(option) {
                if (typeof option == "boolean") {
                    this.wholeWords = option;
                } else {
                    this.wholeWords = !this.wholeWords;
                }
                saveState(this);
            },

            setHighlighting : function(option) {
                if (typeof option == "boolean") {
                    this.highlightEnable = option;
                } else {
                    this.highlightEnable = !this.highlightEnable;
                }
                saveState(this);
            },

            setCaseSensitive : function(option) {
                if (typeof option == "boolean") {
                    this.caseSensitive = option;
                } else {
                    this.caseSensitive = !this.caseSensitive;
                }
                saveState(this);
            }
		},

		data:{
			stopWordsList: new Array(),
            searchProviders: new Array(),

            registerSearchProvider : function(provider) {
                this.searchProviders[provider.getName()] = provider;
            },

            registerSearchDataProvider : function(name, dataProvider) {
                name = name != "" ? name : "default";
                var searchProvider = this.getSearchProvider(name);
                if (searchProvider) {
                    searchProvider.registerDataProvider(dataProvider);
                }
            },

            getSearchProvider : function(name) {
                name = name != "" ? name : "default";
                return this.searchProviders[name];
            }
		},

		buffer:{
			filteredStopWords:"",
			querryWordList: new Array()
		},

        SearchString: function(query) {
            var preQuery = prepareQuery(query);
            var preQueryArr = preQuery.split(" ");
            var queryArr = filterQuery(preQueryArr, this.buffer, this.data);
            var stemmedQueryArr = this.SearchStemmer.stem(queryArr);
            var regexpArr = generateRegExpArray(queryArr, stemmedQueryArr, this.config);

            var results = new this.ResultList();
            for (var i in this.data.searchProviders) {
                var searchProvider = this.data.searchProviders[i];
                if (searchProvider.isEnabled()) {
                    var resultsFromBook = searchProvider.SearchString(regexpArr, queryArr, this.config.searchMethod);
                    results.mergeResults(resultsFromBook);
                }
            }

            results.sortResults();
            return results;
        },

        doSearch : function(query, searchFrame) {
            this.ui.clearWarnings();
            this.config.searchTerm = query;
            saveState(this.config);
            var results = this.SearchString(query);
            if (this.buffer.filteredStopWords != "") {
                this.ui.showWarning("filtered.stopwords", this.buffer.filteredStopWords);
            }
            this.ui.displaySearchResult(results, searchFrame, this.config);
        },

        SearchResult: function(href, title, rank, bookName, description, ancestry) {
            return {
                href : href,
                title : title,
                rank: rank,
                bookName: bookName,
                description: description,
                ancestry: ancestry
            };
        },

        ResultList : function () {
            var list = new Array();
            var searchQuery = "";

            function quicksort(m, n, desc) {
                if (n <= m + 1) return;
                if ((n - m) == 2) {
                    if (compare(getElement(n - 1), getElement(m), desc)) exchange(n - 1, m);
                    return;
                }
                var i = m + 1;
                var j = n - 1;
                if (compare(getElement(m), getElement(i), desc)) exchange(i, m);
                if (compare(getElement(j), getElement(m), desc)) exchange(m, j);
                if (compare(getElement(m), getElement(i), desc)) exchange(i, m);
                var pivot = getElement(m);
                while (true) {
                    j--;
                    while (compare(pivot, getElement(j), desc)) j--;
                    i++;
                    while (compare(getElement(i), pivot, desc)) i++;
                    if (j <= i) break;
                    exchange(i, j);
                }
                exchange(m, j);
                if ((j - m) < (n - j)) {
                    quicksort(m, j, desc);
                    quicksort(j + 1, n, desc);
                } else {
                    quicksort(j + 1, n, desc);
                    quicksort(m, j, desc);
                }
            };

            function getElement(i) {
                return list[i].rank;
            };

            function compare(val1, val2, desc) {
                return (desc) ? val1 > val2 : val1 < val2;
            };

            function exchange(i, j) {
                // exchange adjacent
                // var tResult = new Array(4);
                var exchangeBuffer = list[i];
                list[i] = list[j];
                list[j] = exchangeBuffer;
            };

            return {
                addResult : function(searchResult) {
                    list[list.length] = searchResult;
                },

                sortResults : function(order) {
                    if (order === undefined) {
                        order = true;
                    }
                    quicksort(0, list.length, order);
                },

                getResults : function() {
                    return list;
                },

                addSearchQuery : function(query) {
                    searchQuery = query;
                },

                getSearchQuery : function() {
                    return searchQuery;
                },

                mergeResults : function(resultObject) {
                    var results = resultObject.getResults();
                    for (var i = 0; i< results.length; i++) {
                        list[list.length] = results[i];
                    }
                    searchQuery = resultObject.getSearchQuery();
                }
            };
        },

        SearchStemmer : function() {
            var step2list = new Array();
            step2list["ational"]="ate";
            step2list["tional"]="tion";
            step2list["enci"]="ence";
            step2list["anci"]="ance";
            step2list["izer"]="ize";
            step2list["bli"]="ble";
            step2list["alli"]="al";
            step2list["entli"]="ent";
            step2list["eli"]="e";
            step2list["ousli"]="ous";
            step2list["ization"]="ize";
            step2list["ation"]="ate";
            step2list["ator"]="ate";
            step2list["alism"]="al";
            step2list["iveness"]="ive";
            step2list["fulness"]="ful";
            step2list["ousness"]="ous";
            step2list["aliti"]="al";
            step2list["iviti"]="ive";
            step2list["biliti"]="ble";
            step2list["logi"]="log";

            var step3list = new Array();
            step3list["icate"]="ic";
            step3list["ative"]="";
            step3list["alize"]="al";
            step3list["iciti"]="ic";
            step3list["ical"]="ic";
            step3list["ful"]="";
            step3list["ness"]="";

            var c = "[^aeiou]";          // consonant
            var v = "[aeiouy]";          // vowel
            var C = c + "[^aeiouy]*";    // consonant sequence
            var V = v + "[aeiou]*";      // vowel sequence

            var mgr0 = "^(" + C + ")?" + V + C;               // [C]VC... is m>0
            var meq1 = "^(" + C + ")?" + V + C + "(" + V + ")?$";  // [C]VC[V] is m=1
            var mgr1 = "^(" + C + ")?" + V + C + V + C;       // [C]VCVC... is m>1
            var s_v   = "^(" + C + ")?" + v;                   // vowel in stem

            function stemWord(w) {
                var stem;
                var suffix;
                var firstch;

                if (w.length < 3) { return w; }

                   var re;
                   var re2;
                   var re3;
                   var re4;

                firstch = w.substr(0,1);
                if (firstch == "y") {
                    w = firstch.toUpperCase() + w.substr(1);
                }

                // Step 1a
                   re = /^(.+?)(ss|i)es$/;
                   re2 = /^(.+?)([^s])s$/;

                   if (re.test(w)) { w = w.replace(re,"$1$2"); }
                   else if (re2.test(w)) {	w = w.replace(re2,"$1$2"); }

                // Step 1b
                re = /^(.+?)eed$/;
                re2 = /^(.+?)(ed|ing)$/;
                if (re.test(w)) {
                    var fp1 = re.exec(w);
                    re = new RegExp(mgr0);
                    if (re.test(fp1[1])) {
                        re = /.$/;
                        w = w.replace(re,"");
                    }
                } else if (re2.test(w)) {
                    var fp2 = re2.exec(w);
                    stem = fp2[1];
                    re2 = new RegExp(s_v);
                    if (re2.test(stem)) {
                        w = stem;
                        re2 = /(at|bl|iz)$/;
                        re3 = new RegExp("([^aeiouylsz])\\1$");
                        re4 = new RegExp("^" + C + v + "[^aeiouwxy]$");
                        if (re2.test(w)) {	w = w + "e"; }
                        else if (re3.test(w)) { re = /.$/; w = w.replace(re,""); }
                        else if (re4.test(w)) { w = w + "e"; }
                    }
                }

                // Step 1c
                re = /^(.+?)y$/;
                if (re.test(w)) {
                    var fp3 = re.exec(w);
                    stem = fp3[1];
                    re = new RegExp(s_v);
                    if (re.test(stem)) { w = stem + "i"; }
                }

                // Step 2
                re = /^(.+?)(ational|tional|enci|anci|izer|bli|alli|entli|eli|ousli|ization|ation|ator|alism|iveness|fulness|ousness|aliti|iviti|biliti|logi)$/;
                if (re.test(w)) {
                    var fp4 = re.exec(w);
                    stem = fp4[1];
                    suffix = fp4[2];
                    re = new RegExp(mgr0);
                    if (re.test(stem)) {
                        w = stem + step2list[suffix];
                    }
                }

                // Step 3
                re = /^(.+?)(icate|ative|alize|iciti|ical|ful|ness)$/;
                if (re.test(w)) {
                    var fp5 = re.exec(w);
                    stem = fp5[1];
                    suffix = fp5[2];
                    re = new RegExp(mgr0);
                    if (re.test(stem)) {
                        w = stem + step3list[suffix];
                    }
                }

                // Step 4
                re = /^(.+?)(al|ance|ence|er|ic|able|ible|ant|ement|ment|ent|ou|ism|ate|iti|ous|ive|ize)$/;
                re2 = /^(.+?)(s|t)(ion)$/;
                if (re.test(w)) {
                    var fp6 = re.exec(w);
                    stem = fp6[1];
                    re = new RegExp(mgr1);
                    if (re.test(stem)) {
                        w = stem;
                    }
                } else if (re2.test(w)) {
                    var fp7 = re2.exec(w);
                    stem = fp7[1] + fp7[2];
                    re2 = new RegExp(mgr1);
                    if (re2.test(stem)) {
                        w = stem;
                    }
                }

                // Step 5
                re = /^(.+?)e$/;
                if (re.test(w)) {
                    var fp8 = re.exec(w);
                    stem = fp8[1];
                    re = new RegExp(mgr1);
                    re2 = new RegExp(meq1);
                    re3 = new RegExp("^" + C + v + "[^aeiouwxy]$");
                    if (re.test(stem) || (re2.test(stem) && !(re3.test(stem)))) {
                        w = stem;
                    }
                }

                re = /ll$/;
                re2 = new RegExp(mgr1);
                if (re.test(w) && re2.test(w)) {
                    re = /.$/;
                    w = w.replace(re,"");
                }

                // and turn initial Y back to y

                if (firstch == "y") {
                    w = firstch.toLowerCase() + w.substr(1);
                }

                return w;

            }

            return {
                stem : function(words) {
                    if (top.HlpSys.config && top.HlpSys.config.locale == 'en_US') {
                        var resultWords = new Array();
                        for (var i = 0; i < words.length; i++) {
                            resultWords[resultWords.length] = stemWord(words[i]);
                        }
                        return resultWords;
                    } else {
                        return words;
                    }
                }
            };
        }(),

        ui : {
            messages : new Array(),

            showedMessages : new Array(),

            //Shows warning from on of predefined messages.
            //@param message - messege type to show in warning block.
            //@param parameters - that should be added to the message
            showWarning : function(message, param) {
                var result_message = "";
                if (!top.HlpSys.search.ui.messages[message]) {
                    return;
                }

                if (top.HlpSys.search.ui.showedMessages[message]) {
                    return;
                }

                switch (message) {
                    case "filtered.stopwords" : {
                        result_message = top.HlpSys.search.ui.messages[message] + param;
                        break;
                    }
                    default: {
                        result_message = top.HlpSys.search.ui.messages[message];
                        break;
                    }
                }

                if (result_message != "") {
                    top.HlpSys.search.ui.showedMessages[message] = true;
                    var warningBlock = workingDocument.getElementById("warningBlock");
                    var warning = workingDocument.createElement("div");
                    warning.setAttribute("class", "warning");

                    var text = workingDocument.createTextNode(result_message);
                    warning.appendChild(text);
                    warningBlock.appendChild(warning);
                }
            },

            //Clear warnings displayed during search
            clearWarnings : function() {
                var warningBlock = workingDocument.getElementById("warningBlock");
                while (warningBlock.hasChildNodes()) {
                    var node = warningBlock.firstChild;
                    warningBlock.removeChild(node);
                }
                top.HlpSys.search.ui.showedMessages = new Array();
            },

            //Update progress bar of search process.
            //Not Implemented
            updateProgress: function() {

            },

            //Displays result of the search.
            //@param results  - ResultList objects that contains all results and search query for highlithing.
            //@param resultsDocument - document which was passed to the function doSearch. Usually we sending document where results should be displayed.
            displaySearchResult: (function() {
                if (platformType === 'mobile') {
                    return displaySearchResultMobile;
                } else {
                    return displaySearchResultDesktop;
                }
            })(),

            viewResult: (function() {
                if (platformType === 'mobile') {
                    return viewResultMobile;
                } else {
                    return viewResultDesktop;
                }
            })()

        }
    };
}();

// Used to include a JS file dynamically
function include_js(src) {
    var type = "text/JavaScript";
    document.write("<script src = \"" + src + "\" type = \"" + type + "\"></script>");
};
// SIG // Begin signature block
// SIG // MIIZNgYJKoZIhvcNAQcCoIIZJzCCGSMCAQExCzAJBgUr
// SIG // DgMCGgUAMGcGCisGAQQBgjcCAQSgWTBXMDIGCisGAQQB
// SIG // gjcCAR4wJAIBAQQQEODJBs441BGiowAQS9NQkAIBAAIB
// SIG // AAIBAAIBAAIBADAhMAkGBSsOAwIaBQAEFPKQ8mlfxJtA
// SIG // fW8/tMl1ti9RrDDIoIIUMDCCA+4wggNXoAMCAQICEH6T
// SIG // 6/t8xk5Z6kuad9QG/DswDQYJKoZIhvcNAQEFBQAwgYsx
// SIG // CzAJBgNVBAYTAlpBMRUwEwYDVQQIEwxXZXN0ZXJuIENh
// SIG // cGUxFDASBgNVBAcTC0R1cmJhbnZpbGxlMQ8wDQYDVQQK
// SIG // EwZUaGF3dGUxHTAbBgNVBAsTFFRoYXd0ZSBDZXJ0aWZp
// SIG // Y2F0aW9uMR8wHQYDVQQDExZUaGF3dGUgVGltZXN0YW1w
// SIG // aW5nIENBMB4XDTEyMTIyMTAwMDAwMFoXDTIwMTIzMDIz
// SIG // NTk1OVowXjELMAkGA1UEBhMCVVMxHTAbBgNVBAoTFFN5
// SIG // bWFudGVjIENvcnBvcmF0aW9uMTAwLgYDVQQDEydTeW1h
// SIG // bnRlYyBUaW1lIFN0YW1waW5nIFNlcnZpY2VzIENBIC0g
// SIG // RzIwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
// SIG // AQCxrLNJVEuXHBIK2CV5kSJXKm/cuCbEQ3Nrwr8uUFr7
// SIG // FMJ2jkMBJUO0oeJF9Oi3e8N0zCLXtJQAAvdN7b+0t0Qk
// SIG // a81fRTvRRM5DEnMXgotptCvLmR6schsmTXEfsTHd+1Fh
// SIG // AlOmqvVJLAV4RaUvic7nmef+jOJXPz3GktxK+Hsz5HkK
// SIG // +/B1iEGc/8UDUZmq12yfk2mHZSmDhcJgFMTIyTsU2sCB
// SIG // 8B8NdN6SIqvK9/t0fCfm90obf6fDni2uiuqm5qonFn1h
// SIG // 95hxEbziUKFL5V365Q6nLJ+qZSDT2JboyHylTkhE/xni
// SIG // RAeSC9dohIBdanhkRc1gRn5UwRN8xXnxycFxAgMBAAGj
// SIG // gfowgfcwHQYDVR0OBBYEFF+a9W5czMx0mtTdfe8/2+xM
// SIG // gC7dMDIGCCsGAQUFBwEBBCYwJDAiBggrBgEFBQcwAYYW
// SIG // aHR0cDovL29jc3AudGhhd3RlLmNvbTASBgNVHRMBAf8E
// SIG // CDAGAQH/AgEAMD8GA1UdHwQ4MDYwNKAyoDCGLmh0dHA6
// SIG // Ly9jcmwudGhhd3RlLmNvbS9UaGF3dGVUaW1lc3RhbXBp
// SIG // bmdDQS5jcmwwEwYDVR0lBAwwCgYIKwYBBQUHAwgwDgYD
// SIG // VR0PAQH/BAQDAgEGMCgGA1UdEQQhMB+kHTAbMRkwFwYD
// SIG // VQQDExBUaW1lU3RhbXAtMjA0OC0xMA0GCSqGSIb3DQEB
// SIG // BQUAA4GBAAMJm495739ZMKrvaLX64wkdu0+CBl03X6ZS
// SIG // nxaN6hySCURu9W3rWHww6PlpjSNzCxJvR6muORH4KrGb
// SIG // sBrDjutZlgCtzgxNstAxpghcKnr84nodV0yoZRjpeUBi
// SIG // JZZux8c3aoMhCI5B6t3ZVz8dd0mHKhYGXqY4aiISo1EZ
// SIG // g362MIIEozCCA4ugAwIBAgIQDs/0OMj+vzVuBNhqmBsa
// SIG // UDANBgkqhkiG9w0BAQUFADBeMQswCQYDVQQGEwJVUzEd
// SIG // MBsGA1UEChMUU3ltYW50ZWMgQ29ycG9yYXRpb24xMDAu
// SIG // BgNVBAMTJ1N5bWFudGVjIFRpbWUgU3RhbXBpbmcgU2Vy
// SIG // dmljZXMgQ0EgLSBHMjAeFw0xMjEwMTgwMDAwMDBaFw0y
// SIG // MDEyMjkyMzU5NTlaMGIxCzAJBgNVBAYTAlVTMR0wGwYD
// SIG // VQQKExRTeW1hbnRlYyBDb3Jwb3JhdGlvbjE0MDIGA1UE
// SIG // AxMrU3ltYW50ZWMgVGltZSBTdGFtcGluZyBTZXJ2aWNl
// SIG // cyBTaWduZXIgLSBHNDCCASIwDQYJKoZIhvcNAQEBBQAD
// SIG // ggEPADCCAQoCggEBAKJjCzlEuLsjp0RJuw7/ofBhClOT
// SIG // sJjbrSwPSsVu/4Y8U1UPFc4EPyv9qZaW2b5heQtbyUyG
// SIG // duXgQ0sile7CK0PBn9hotI5AT+6FOLkRxSPyZFjwFTJv
// SIG // TlehroikAtcqHs1L4d1j1ReJMluwXplaqJ0oUA4X7pbb
// SIG // YTtFUR3PElYLkkf8q672Zj1HrHBy55LnX80QucSDZJQZ
// SIG // vSWA4ejSIqXQugJ6oXeTW2XD7hd0vEGGKtwITIySjJEt
// SIG // nndEH2jWqHR32w5bMotWizO92WPISZ06xcXqMwvS8aMb
// SIG // 9Iu+2bNXizveBKd6IrIkri7HcMW+ToMmCPsLvalPmQjh
// SIG // EChyqs0CAwEAAaOCAVcwggFTMAwGA1UdEwEB/wQCMAAw
// SIG // FgYDVR0lAQH/BAwwCgYIKwYBBQUHAwgwDgYDVR0PAQH/
// SIG // BAQDAgeAMHMGCCsGAQUFBwEBBGcwZTAqBggrBgEFBQcw
// SIG // AYYeaHR0cDovL3RzLW9jc3Aud3Muc3ltYW50ZWMuY29t
// SIG // MDcGCCsGAQUFBzAChitodHRwOi8vdHMtYWlhLndzLnN5
// SIG // bWFudGVjLmNvbS90c3MtY2EtZzIuY2VyMDwGA1UdHwQ1
// SIG // MDMwMaAvoC2GK2h0dHA6Ly90cy1jcmwud3Muc3ltYW50
// SIG // ZWMuY29tL3Rzcy1jYS1nMi5jcmwwKAYDVR0RBCEwH6Qd
// SIG // MBsxGTAXBgNVBAMTEFRpbWVTdGFtcC0yMDQ4LTIwHQYD
// SIG // VR0OBBYEFEbGaaMOShQe1UzaUmMXP142vA3mMB8GA1Ud
// SIG // IwQYMBaAFF+a9W5czMx0mtTdfe8/2+xMgC7dMA0GCSqG
// SIG // SIb3DQEBBQUAA4IBAQB4O7SRKgBM8I9iMDd4o4QnB28Y
// SIG // st4l3KDUlAOqhk4ln5pAAxzdzuN5yyFoBtq2MrRtv/Qs
// SIG // JmMz5ElkbQ3mw2cO9wWkNWx8iRbG6bLfsundIMZxD82V
// SIG // dNy2XN69Nx9DeOZ4tc0oBCCjqvFLxIgpkQ6A0RH83Vx2
// SIG // bk9eDkVGQW4NsOo4mrE62glxEPwcebSAe6xp9P2ctgwW
// SIG // K/F/Wwk9m1viFsoTgW0ALjgNqCmPLOGy9FqpAa8VnCwv
// SIG // SRvbIrvD/niUUcOGsYKIXfA9tFGheTMrLnu53CAJE3Hr
// SIG // ahlbz+ilMFcsiUk/uc9/yb8+ImhjU5q9aXSsxR08f5Lg
// SIG // w7wc2AR1MIIFhTCCBG2gAwIBAgIQKcFbP6rNUmpOZ708
// SIG // Tn4/8jANBgkqhkiG9w0BAQUFADCBtDELMAkGA1UEBhMC
// SIG // VVMxFzAVBgNVBAoTDlZlcmlTaWduLCBJbmMuMR8wHQYD
// SIG // VQQLExZWZXJpU2lnbiBUcnVzdCBOZXR3b3JrMTswOQYD
// SIG // VQQLEzJUZXJtcyBvZiB1c2UgYXQgaHR0cHM6Ly93d3cu
// SIG // dmVyaXNpZ24uY29tL3JwYSAoYykxMDEuMCwGA1UEAxMl
// SIG // VmVyaVNpZ24gQ2xhc3MgMyBDb2RlIFNpZ25pbmcgMjAx
// SIG // MCBDQTAeFw0xMjA3MjUwMDAwMDBaFw0xNTA5MjAyMzU5
// SIG // NTlaMIHIMQswCQYDVQQGEwJVUzETMBEGA1UECBMKQ2Fs
// SIG // aWZvcm5pYTETMBEGA1UEBxMKU2FuIFJhZmFlbDEWMBQG
// SIG // A1UEChQNQXV0b2Rlc2ssIEluYzE+MDwGA1UECxM1RGln
// SIG // aXRhbCBJRCBDbGFzcyAzIC0gTWljcm9zb2Z0IFNvZnR3
// SIG // YXJlIFZhbGlkYXRpb24gdjIxHzAdBgNVBAsUFkRlc2ln
// SIG // biBTb2x1dGlvbnMgR3JvdXAxFjAUBgNVBAMUDUF1dG9k
// SIG // ZXNrLCBJbmMwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAw
// SIG // ggEKAoIBAQCoYmDrmd0Gq8ezSsDlfgaJFEFplNPNhWzM
// SIG // 2uFQaYAB/ggpQ11+N4B6ao+TqrNIWDIqt3JKhaU889nx
// SIG // l/7teWGwuOurstI2Z0bEDhXiXam/bicK2HVLyntliQ+6
// SIG // tT+nlgfN8tgB2NzM0BpE1YCnU2b6DwQw4V7BV+/F//83
// SIG // yGFOpePlumzXxNw9EKWkaq81slmmTxf7UxZgP9PGbLw8
// SIG // gLAPk4PTJI97+5BBqhkLb1YqSfWn3PNMfsNKhw/VwAN0
// SIG // dRKeM6H8SkOdz+osr+NyH86lsKQuics4fwK5uFSHQHsI
// SIG // t6Z0tqWvminRqceUi9ugRlGryh9X1ZqCqfL/ggdzYa3Z
// SIG // AgMBAAGjggF7MIIBdzAJBgNVHRMEAjAAMA4GA1UdDwEB
// SIG // /wQEAwIHgDBABgNVHR8EOTA3MDWgM6Axhi9odHRwOi8v
// SIG // Y3NjMy0yMDEwLWNybC52ZXJpc2lnbi5jb20vQ1NDMy0y
// SIG // MDEwLmNybDBEBgNVHSAEPTA7MDkGC2CGSAGG+EUBBxcD
// SIG // MCowKAYIKwYBBQUHAgEWHGh0dHBzOi8vd3d3LnZlcmlz
// SIG // aWduLmNvbS9ycGEwEwYDVR0lBAwwCgYIKwYBBQUHAwMw
// SIG // cQYIKwYBBQUHAQEEZTBjMCQGCCsGAQUFBzABhhhodHRw
// SIG // Oi8vb2NzcC52ZXJpc2lnbi5jb20wOwYIKwYBBQUHMAKG
// SIG // L2h0dHA6Ly9jc2MzLTIwMTAtYWlhLnZlcmlzaWduLmNv
// SIG // bS9DU0MzLTIwMTAuY2VyMB8GA1UdIwQYMBaAFM+Zqep7
// SIG // JvRLyY6P1/AFJu/j0qedMBEGCWCGSAGG+EIBAQQEAwIE
// SIG // EDAWBgorBgEEAYI3AgEbBAgwBgEBAAEB/zANBgkqhkiG
// SIG // 9w0BAQUFAAOCAQEA2OkGvuiY7TyI6yVTQAYmTO+MpOFG
// SIG // C8MflHSbofJiuLxrS1KXbkzsAPFPPsU1ouftFhsXFtDQ
// SIG // 8rMTq/jwugTpbJUREV0buEkLl8AKRhYQTKBKg1I/puBv
// SIG // bkJocDE0pRwtBz3xSlXXEwyYPcbCOnrM3OZ5bKx1Qiii
// SIG // vixlcGWhO3ws904ssutPFf4mV5PDi3U2Yp1HgbBK/Um/
// SIG // FLr6YAYeZaA8KY1CfQEisF3UKTwm72d7S+fJf++SOGea
// SIG // K0kumehVcbavQJTOVebuZ9V+qU0nk1lMrqve9BnQK69B
// SIG // QqNZu77vCO0wm81cfynAxkOYKZG3idY47qPJOgXKkwmI
// SIG // 2+92ozCCBgowggTyoAMCAQICEFIA5aolVvwahu2WydRL
// SIG // M8cwDQYJKoZIhvcNAQEFBQAwgcoxCzAJBgNVBAYTAlVT
// SIG // MRcwFQYDVQQKEw5WZXJpU2lnbiwgSW5jLjEfMB0GA1UE
// SIG // CxMWVmVyaVNpZ24gVHJ1c3QgTmV0d29yazE6MDgGA1UE
// SIG // CxMxKGMpIDIwMDYgVmVyaVNpZ24sIEluYy4gLSBGb3Ig
// SIG // YXV0aG9yaXplZCB1c2Ugb25seTFFMEMGA1UEAxM8VmVy
// SIG // aVNpZ24gQ2xhc3MgMyBQdWJsaWMgUHJpbWFyeSBDZXJ0
// SIG // aWZpY2F0aW9uIEF1dGhvcml0eSAtIEc1MB4XDTEwMDIw
// SIG // ODAwMDAwMFoXDTIwMDIwNzIzNTk1OVowgbQxCzAJBgNV
// SIG // BAYTAlVTMRcwFQYDVQQKEw5WZXJpU2lnbiwgSW5jLjEf
// SIG // MB0GA1UECxMWVmVyaVNpZ24gVHJ1c3QgTmV0d29yazE7
// SIG // MDkGA1UECxMyVGVybXMgb2YgdXNlIGF0IGh0dHBzOi8v
// SIG // d3d3LnZlcmlzaWduLmNvbS9ycGEgKGMpMTAxLjAsBgNV
// SIG // BAMTJVZlcmlTaWduIENsYXNzIDMgQ29kZSBTaWduaW5n
// SIG // IDIwMTAgQ0EwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAw
// SIG // ggEKAoIBAQD1I0tepdeKuzLp1Ff37+THJn6tGZj+qJ19
// SIG // lPY2axDXdYEwfwRof8srdR7NHQiM32mUpzejnHuA4Jnh
// SIG // 7jdNX847FO6G1ND1JzW8JQs4p4xjnRejCKWrsPvNamKC
// SIG // TNUh2hvZ8eOEO4oqT4VbkAFPyad2EH8nA3y+rn59wd35
// SIG // BbwbSJxp58CkPDxBAD7fluXF5JRx1lUBxwAmSkA8taEm
// SIG // qQynbYCOkCV7z78/HOsvlvrlh3fGtVayejtUMFMb32I0
// SIG // /x7R9FqTKIXlTBdOflv9pJOZf9/N76R17+8V9kfn+Bly
// SIG // 2C40Gqa0p0x+vbtPDD1X8TDWpjaO1oB21xkupc1+NC2J
// SIG // AgMBAAGjggH+MIIB+jASBgNVHRMBAf8ECDAGAQH/AgEA
// SIG // MHAGA1UdIARpMGcwZQYLYIZIAYb4RQEHFwMwVjAoBggr
// SIG // BgEFBQcCARYcaHR0cHM6Ly93d3cudmVyaXNpZ24uY29t
// SIG // L2NwczAqBggrBgEFBQcCAjAeGhxodHRwczovL3d3dy52
// SIG // ZXJpc2lnbi5jb20vcnBhMA4GA1UdDwEB/wQEAwIBBjBt
// SIG // BggrBgEFBQcBDARhMF+hXaBbMFkwVzBVFglpbWFnZS9n
// SIG // aWYwITAfMAcGBSsOAwIaBBSP5dMahqyNjmvDz4Bq1EgY
// SIG // LHsZLjAlFiNodHRwOi8vbG9nby52ZXJpc2lnbi5jb20v
// SIG // dnNsb2dvLmdpZjA0BgNVHR8ELTArMCmgJ6AlhiNodHRw
// SIG // Oi8vY3JsLnZlcmlzaWduLmNvbS9wY2EzLWc1LmNybDA0
// SIG // BggrBgEFBQcBAQQoMCYwJAYIKwYBBQUHMAGGGGh0dHA6
// SIG // Ly9vY3NwLnZlcmlzaWduLmNvbTAdBgNVHSUEFjAUBggr
// SIG // BgEFBQcDAgYIKwYBBQUHAwMwKAYDVR0RBCEwH6QdMBsx
// SIG // GTAXBgNVBAMTEFZlcmlTaWduTVBLSS0yLTgwHQYDVR0O
// SIG // BBYEFM+Zqep7JvRLyY6P1/AFJu/j0qedMB8GA1UdIwQY
// SIG // MBaAFH/TZafC3ey78DAJ80M5+gKvMzEzMA0GCSqGSIb3
// SIG // DQEBBQUAA4IBAQBWIuY0pMRhy0i5Aa1WqGQP2YyRxLvM
// SIG // DOWteqAif99HOEotbNF/cRp87HCpsfBP5A8MU/oVXv50
// SIG // mEkkhYEmHJEUR7BMY4y7oTTUxkXoDYUmcwPQqYxkbdxx
// SIG // kuZFBWAVWVE5/FgUa/7UpO15awgMQXLnNyIGCb4j6T9E
// SIG // mh7pYZ3MsZBc/D3SjaxCPWU21LQ9QCiPmxDPIybMSyDL
// SIG // kB9djEw0yjzY5TfWb6UgvTTrJtmuDefFmvehtCGRM2+G
// SIG // 6Fi7JXx0Dlj+dRtjP84xfJuPG5aexVN2hFucrZH6rO2T
// SIG // ul3IIVPCglNjrxINUIcRGz1UUpaKLJw9khoImgUux5Ol
// SIG // SJHTMYIEcjCCBG4CAQEwgckwgbQxCzAJBgNVBAYTAlVT
// SIG // MRcwFQYDVQQKEw5WZXJpU2lnbiwgSW5jLjEfMB0GA1UE
// SIG // CxMWVmVyaVNpZ24gVHJ1c3QgTmV0d29yazE7MDkGA1UE
// SIG // CxMyVGVybXMgb2YgdXNlIGF0IGh0dHBzOi8vd3d3LnZl
// SIG // cmlzaWduLmNvbS9ycGEgKGMpMTAxLjAsBgNVBAMTJVZl
// SIG // cmlTaWduIENsYXNzIDMgQ29kZSBTaWduaW5nIDIwMTAg
// SIG // Q0ECECnBWz+qzVJqTme9PE5+P/IwCQYFKw4DAhoFAKBw
// SIG // MBAGCisGAQQBgjcCAQwxAjAAMBkGCSqGSIb3DQEJAzEM
// SIG // BgorBgEEAYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgor
// SIG // BgEEAYI3AgEVMCMGCSqGSIb3DQEJBDEWBBSeBPvzpZ0h
// SIG // tBwTFdhJZdgI8W9/gTANBgkqhkiG9w0BAQEFAASCAQCf
// SIG // tZlZY/EpDUP3VaH8Ivk1xznUZ+0hHW9LOOij+V8i+KQl
// SIG // gI16yxgLF7HXVtxKBCaSZb3DdUTV/sqIYSvlQvl0uPUz
// SIG // 7B+lBd70WKg/w5fpiEYbzbPf0SNAnvFGUySNnWvdJf28
// SIG // kqKPhb0ghHpNT1aTSBRdghYEWtASWKZCgNpkxonPzG7G
// SIG // fCFecWVng4mIDOXxrJHwBQcC0eD03IY0Jlb8+nBcYqk/
// SIG // IiLiJ+aU1m6mr0aYglwygsR3GHAFh9SX/IgN5it+GTg+
// SIG // RN7nHlcU2cbynLZACEM7xGD3eWFU4wnwdFu7c0BpFLZJ
// SIG // +SLKlmBdytSvdRx6Qif5EgEr4iybqr9QoYICCzCCAgcG
// SIG // CSqGSIb3DQEJBjGCAfgwggH0AgEBMHIwXjELMAkGA1UE
// SIG // BhMCVVMxHTAbBgNVBAoTFFN5bWFudGVjIENvcnBvcmF0
// SIG // aW9uMTAwLgYDVQQDEydTeW1hbnRlYyBUaW1lIFN0YW1w
// SIG // aW5nIFNlcnZpY2VzIENBIC0gRzICEA7P9DjI/r81bgTY
// SIG // apgbGlAwCQYFKw4DAhoFAKBdMBgGCSqGSIb3DQEJAzEL
// SIG // BgkqhkiG9w0BBwEwHAYJKoZIhvcNAQkFMQ8XDTE1MDMw
// SIG // NTE1MDI0OFowIwYJKoZIhvcNAQkEMRYEFPoK5wNmfb4m
// SIG // 6mfSD/SrIFbGyGKnMA0GCSqGSIb3DQEBAQUABIIBAFQc
// SIG // GJn0SY0BZdXi+ugbfB9Y7MTv2cRJuirKcdUCBlRFGwRW
// SIG // im6EDXAzxbHvRswHf65xIISON1xCvtvDmS+/Jtcsqp33
// SIG // N9ErFL6JQKC3YaHqW31I4PDJWk6slLoZ//Znq3qQjhh9
// SIG // Q/lDB9dDzc3FE+N2QtHBE7s8t4Vj54ByXajwdddDiO8Z
// SIG // jT49qu+tOykR1kETwP/5Bkfe1RDiQZmiQHiYHuXOkDbX
// SIG // v7ac5XNzu+JapYS4uDEeD4htsfWljCdphuIaVTpO2CND
// SIG // w63BnOqdrcR0zT2j0741I3h+7u1WY5dx5+eGu+v+9qmW
// SIG // aVuzoZ2M2HCg3OPzokv7xEArY+tSGN8=
// SIG // End signature block
