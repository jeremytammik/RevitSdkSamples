if (top.HlpSys === undefined) {
    top.HlpSys = new Object();
}

top.HlpSys.highlight = function(){
    var searchObj;
    var highlightObj;

    function decodeReferrer(q, config) {
        var query = new Array();

        if (q != null && q.length > 0) {
            query[0] = q[0];
            for (var i1 = 0; i1 < q.length; i1++) {
                query[query.length] = q[i1];
                if (i1 != 0) {
                    query[0] += "[^\\w]+" + q[i1];
                }
            }

        } else {
            return null;
        }

        var caseSens = config.caseSensitive;
        var exact = config.wholeWords;
        var qre = new Array();
        for (var i2 = 0; i2 < query.length; i2 ++) {
            query[i2] = caseSens ? query[i2] : query[i2].toLowerCase();
            if (exact)
                qre.push('\\b' + query[i2] + '\\b');
            else
                qre.push(query[i2]);
        }

        for (var i3 = 0; i3 < qre.length; i3 ++) {
            qre[i3] = new RegExp(qre[i3], caseSens ? "" : "i");
        }

        return qre;

    };

    function hiliteElement(elm, query) {
        if (!query || elm.childNodes.length == 0)
            return;

        var qre = query;
        var searchMethod = searchObj.config.searchMethod;

        var textproc = function(node, querryNum) {
            var q = querryNum;
            var match = null;
            var continuedSearch = true;
            if (searchMethod == 'phrase') {
                if (querryNum == 0) {
                    match = qre[q].exec(node.data);
                    q++;
                    querryNum++;
                    continuedSearch = false;
                }
                if (match) {
                    var val = match[0];
                    var k = '';
                    var node2 = node.splitText(match.index);
                    var node3 = node2.splitText(val.length);
                    var span = node.ownerDocument.createElement('SPAN');
                    node.parentNode.replaceChild(span, node2);
                    span.className = highlightObj.config.stylemapper[0];
                    span.appendChild(node2);
                    return span;
                } else {
                    if (qre[q] && qre[q].exec(node.data)) {
                        var words = normalizeSpace(node.data).split(/[\[ | \^ | \$ | \. | \| | \+ | \( | \) | ` | ~ | ! | # | % | & | - | \- | _ | = | \] | { | } | ; | ' | " | < | > | ,]/);
                        var w = 0;
                        var wrongSearch = false;
                        if (q == 1) {
                            for (w = 0; w < words.length; w++) {
                                if (qre[q].exec(words[w])) {
                                    break;
                                }
                            }
                        }
                        while (q < qre.length && w < words.length) {
                            if (qre[q].exec(words[w])) {
                                q++;
                                w++;
                            } else if (searchObj.data.stopWordsList[words[w].toLowerCase()] || words[w].length == 0) {
                                w++;
                            } else {
                                wrongSearch = true;
                                break;
                            }
                        }
                        if (!wrongSearch) {
                            q--;
                            if (q + 1 == qre.length) {
                                var matchS = qre[querryNum].exec(node.data);
                                var node2 = node.splitText(matchS.index);
                                var matchE = qre[q].exec(node2.data);
                                var node3 = node2.splitText(matchE.index + matchE[0].length);
                                var span = node.ownerDocument.createElement('SPAN');
                                node.parentNode.replaceChild(span, node2);
                                span.className = highlightObj.config.stylemapper[0];
                                span.appendChild(node2);
                                return span;
                            } else {
                                var nextNode = getFollowingTextNode(node);
                                if (nextNode != null) {
                                    var nextProcessedNode = textproc(nextNode, q + 1)
                                    if (nextNode != nextProcessedNode) {
                                        var matchS = qre[querryNum].exec(node.data);
                                        var node2 = node.splitText(matchS.index);
                                        var span = node.ownerDocument.createElement('SPAN');
                                        node.parentNode.replaceChild(span, node2);
                                        span.className = highlightObj.config.stylemapper[0];
                                        span.appendChild(node2);
                                        return nextProcessedNode;
                                    }
                                }
                            }
                        } else if (!continuedSearch && w < words.length) {
                            var matchT = (new RegExp(words[w], "i")).exec(node.data)
                            if (matchT) {
                                var node2 = node.splitText(matchT.index + matchT[0].length);
                                if (node.data.length - matchT.index - matchT[0].length == 0) {
                                    return node;
                                } else {
                                    return textproc(node2, 0);
                                }
                            }
                        }
                    }
                    return node;
                }
            } else {
                for (var i = 0; i < qre.length; i++) {
                    match = qre[i].exec(node.data);
                    if (match) {
                        var val = match[0];
                        var node2 = node.splitText(match.index);
                        var node3 = node2.splitText(val.length);
                        var span = node.ownerDocument.createElement('SPAN');
                        node.parentNode.replaceChild(span, node2);
                        span.className = highlightObj.config.stylemapper[0];
                        span.appendChild(node2);
                        return span;
                    }
                }
                return node;
            }
        };
        walkElements(elm.childNodes[0], 1, textproc);
    };

    function walkElements(node, depth, textproc) {
        var skipre = /^(head|script|style|textarea)/i;
        var count = 0;
        while (node && depth > 0) {
            count ++;
            if (count >= highlightObj.config.max_nodes) {
                var handler = function() {
                    walkElements(node, depth, textproc);
                };
                setTimeout(handler, 50);
                return;
            }

            if (node.nodeType == 1) { // ELEMENT_NODE
                if (!skipre.test(node.tagName) && node.childNodes.length > 0) {
                    node = node.childNodes[0];
                    depth ++;
                    continue;
                }
            } else if (node.nodeType == 3) { // TEXT_NODE
                node = textproc(node, 0);
                if (node.parentNode == null) {
                    alert(node.nodeName + ":1" + node.data + ":");
                }
            }

            if (node == null) {
                return;
            }

            if (node.nextSibling) {
                node = node.nextSibling;
            } else {
                while (depth > 0) {
                    node = node.parentNode;
                    depth --;
                    if (node.nextSibling) {
                        node = node.nextSibling;
                        break;
                    }
                }
            }
        }
    };

    function getFollowingTextNode(node) {
        node = getFollowingNode(node);
        if (node) {
            if (node.nodeType == 3 && normalizeSpace(node.data).length > 0) {
                return node;
            } else {
                return getFollowingTextNode(node);
            }
        } else {
            return null;
        }
    };

    function getFollowingNode(node) {
        if (node) {
            if (node.firstChild) {
                return node.firstChild;
            } else if (node.nextSibling) {
                return node.nextSibling;
            } else {
                while (node.parentNode) {
                    node = node.parentNode;
                    if (node.nextSibling) {
                        return node.nextSibling;
                    }
                }
                return null;
            }
        } else return null;
    };

    function normalizeSpace(string) {
        var regexp = new RegExp("[\\s][\\s]+","g");
        while(string.match(regexp)) {
            string = string.replace(regexp," ");
        }
        if (string.length == 1 && string == " ") {
            return "";
        } else {
            return string;
        }
    };

    function disHilite(node, config) {
        if (node != null) {
            if (node.nodeType == 1 || node.nodeType == 9) { // ELEMENT_NODE
                if ((node.nodeName.toLowerCase() == "span") && (node.attributes["class"] != null) && (node.attributes["class"].nodeValue.indexOf(config.stylename) == 0)) {
                    var childs1 = node.childNodes;
                    for (var j = 0; j < childs1.length; j++) {
                        node.parentNode.insertBefore(childs1[j], node);
                    }
                    node.parentNode.removeChild(node);
                } else if (node.childNodes.length > 0) {
                    var childs2 = node.childNodes;
                    for (var i = 0; i < childs2.length; i++) {
                        disHilite(childs2[i], config);
                    }
                }
            }
        }
    }

    function parseUrlParameters(queryString) {
        var args = new Object();
        var pairs = queryString.split(",");                 // Break at comma
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('=');          // Look for "name=value"
            if (pos == -1) continue;                  // If not found, skip
            var argname = pairs[i].substring(0, pos);  // Extract the name
            var value = pairs[i].substring(pos + 1);    // Extract the value
            args[argname] = decodeURIComponent(value);
        }

        return args;
    };



    return {
		config: {
            onload: true,
			elementid: '',
			max_nodes: 500,
			stylename: 'hilite',
			stylemapper: ['hilite','hilite1','hilite2'],
			debug_referrer: ''
		},

        onload: function() {
            if (top.HlpSys.search !== undefined) {
                searchObj = top.HlpSys.search;
                highlightObj = top.HlpSys.highlight;
                var parameters = window.location.search.substring(1);
                var args = parseUrlParameters(parameters);
                var arg = args["highlighting"];
                if (arg && arg.length > 0) {
                    var decodedWords = decodeURIComponent(arg);
                    var words = decodedWords.split("|");
                    var q = decodeReferrer(words, searchObj.config);
                    top.HlpSys.highlight.hilite(window.document, q, searchObj.config.highlightEnable);
                }
            }
        },

		hilite: function(doc, query, enable) {
			if (enable && query.length > 0) {
				if (doc != null) {
					hiliteElement(doc, query);
				}
			} else {
				disHilite(doc, this.config);
			}
		}
    };
}();

if (top.HlpSys.highlight.config.onload) {
    AddOnLoadFunction(top.HlpSys.highlight.onload);
}

// SIG // Begin signature block
// SIG // MIIZNgYJKoZIhvcNAQcCoIIZJzCCGSMCAQExCzAJBgUr
// SIG // DgMCGgUAMGcGCisGAQQBgjcCAQSgWTBXMDIGCisGAQQB
// SIG // gjcCAR4wJAIBAQQQEODJBs441BGiowAQS9NQkAIBAAIB
// SIG // AAIBAAIBAAIBADAhMAkGBSsOAwIaBQAEFE2MzsbLmTfE
// SIG // zKMok2hIh80KFzvooIIUMDCCA+4wggNXoAMCAQICEH6T
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
// SIG // BgEEAYI3AgEVMCMGCSqGSIb3DQEJBDEWBBTwYP/kLzQl
// SIG // 9nf8UhqGyudh1x4N/TANBgkqhkiG9w0BAQEFAASCAQCn
// SIG // 9UWwzZ8EfBc2TmUlKr4NUqTPMQ/OKW+R+BUhPb+MPqH8
// SIG // axivMgx8o/c0anRXNUAoH3qTupMDmzA7mt6MyesJOkH+
// SIG // yWNWp0sTsmYhmAaLAChAKLmoyVmx55TyUw3q61iVVpZj
// SIG // RXRYx6aW0RgoloL0a0lAsibv4jxI+trSMt7KbrIm6kBb
// SIG // C8LEg5TSHcRxV7vBbqKyFSwvYpczzOB1xOdjm8+zaAuU
// SIG // fyhUuWORzVISHOyAHdPGNspVwTdIAufYM2VLUVHuqKTp
// SIG // R6884PzTPcOjSor9tiIg9twkdcV7MjFf8TjyTLBc3Zxi
// SIG // XoTa7Wcao3aa7DUysbVsFWyBj1psC5M8oYICCzCCAgcG
// SIG // CSqGSIb3DQEJBjGCAfgwggH0AgEBMHIwXjELMAkGA1UE
// SIG // BhMCVVMxHTAbBgNVBAoTFFN5bWFudGVjIENvcnBvcmF0
// SIG // aW9uMTAwLgYDVQQDEydTeW1hbnRlYyBUaW1lIFN0YW1w
// SIG // aW5nIFNlcnZpY2VzIENBIC0gRzICEA7P9DjI/r81bgTY
// SIG // apgbGlAwCQYFKw4DAhoFAKBdMBgGCSqGSIb3DQEJAzEL
// SIG // BgkqhkiG9w0BBwEwHAYJKoZIhvcNAQkFMQ8XDTE1MDMw
// SIG // NTE1MDIzNVowIwYJKoZIhvcNAQkEMRYEFIni9DIifWO7
// SIG // hNLmwJTpoRcCLId1MA0GCSqGSIb3DQEBAQUABIIBACBl
// SIG // nvIl0qYZxAuQd8Y9x+5UoQPLLeahmkILO0adA2ee5NCS
// SIG // T8sj3DKT25s7Hqa5J9fKA8gFC6MbWQ3+u+sORY4/c4Xq
// SIG // Sm3R8EvAGh6lBH+a9GRXxOK77irOvm5B71tTkl4Hxgda
// SIG // sEAPTtDjlBS6E6wxJIm8RmmDMl3mAUU/r04lcxXaZDU9
// SIG // Zt6ATEAsRH1BLDBw6GU6eXKaTXuxdVRkU4yhjW/N8VC7
// SIG // vNLnCKK+7bkfYnFL6Y5ChE7SoY7suAFFL8mfXVH7xAt/
// SIG // wh6CqAIzLasE5lxK1S8KOMs2ykel/hfixpLvH5G0/570
// SIG // v0rDgZbFlKJ0WDQBMAZjLVfX2ODFOVU=
// SIG // End signature block
