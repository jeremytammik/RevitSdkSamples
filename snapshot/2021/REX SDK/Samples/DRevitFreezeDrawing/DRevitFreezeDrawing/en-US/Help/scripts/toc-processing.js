var tocImages =  {'ctrlImageLine':'empty_toc_image',
                  'ctrlImageVoid':'empty_toc_image',
                  'ctrlImage0':'empty_toc_image',
                  'ctrlImage1':'empty_toc_image',
                  'ctrlImage2':'empty_toc_image',
                  'ctrlImage3':'empty_toc_image',
                  'ctrlImage4':'empty_toc_image',
                  'ctrlImage8':'plus_toc_image',
                  'ctrlImage9':'plus_toc_image',
                  'ctrlImage10':'plus_toc_image',
                  'ctrlImage11':'plus_toc_image',
                  'ctrlImage12':'minus_toc_image',
                  'ctrlImage13':'minus_toc_image',
                  'ctrlImage14':'minus_toc_image',
                  'topic-opened':'topic_opened_toc_image',
                  'topic-closed':'topic_closed_toc_image',
                  'topic':'topic_toc_image'};
var childTrees=[];
var curViewed;

// ------------------------------------------ Functions for nodes expansion

var chgNodeResult=0;

function expandTOCNode(nodeId)
{
    if(curViewed != null && curViewed.nodeId == nodeId)
        return;
    chgNodeResult = -1;
    expandTOCNodeChilds(nodeId, contentTree);
    return chgNodeResult;
}

function expandTOCNodeChilds(nodeId, parentNode)
{
	if (parentNode.children) {
		for(var i = 0; i < parentNode.children.length; i++)
		{
            var child = parentNode.children[i];
//			if (!child.parent) child.parent = parentNode;
            if (!child.isRendered) initTocNode(child, parentNode);
            if(child.opn) child.OpenCloseNode(false);
			if(nodeId == child.id)
			{
				ExpandNode(child);
				child.select(false);
				chgNodeResult = nodeId;
				return;
			};
			expandTOCNodeChilds(nodeId, child);
		};
	}
}

function renderNode(parent, node){
    parent.innerHTML += node.init();
}

function ExpandAllOpened(nodePointer) {
	if (nodePointer.opn) {
		ExpandNode(nodePointer);
	}
	if (nodePointer.children) {
		for (var i = 0; i<nodePointer.children.length;i++) {
			ExpandAllOpened(nodePointer.children[i]);
		}
	}
}

function ExpandNode(nodePointer)
{
    if(nodePointer.parent == null) {
		return;
	} else {
		ExpandNode(nodePointer.parent);
		nodePointer.OpenCloseNode(false);
	}
}

// ------------------------------------------ TOCNode class

function isFirstNode()
{
    return this.preceding == null;
}

function isLastNode()
{
    return this.following == null;
}

function SetNodeImage(setCtrlImage)
{
    if(setCtrlImage)
    {
        var flags = 0;
        if(this.children) {
			flags |= 0x8;
		}
        if(this.children && this.opn) {
			flags |= 0x4;
		}
        return tocImages['ctrlImage' + flags];
    }
    else
    {
        var currentState;
		if (!this.children) {
			currentState =  tocIcons[this.ic]+"_toc_image";
		} else if (this.opn) {
			currentState = tocIcons[this.ic]+'_opened_toc_image';
		} else {
			currentState = tocIcons[this.ic]+'_closed_toc_image';
		}
        return currentState;
    }
}

function OpenCloseNode(currentState)
{
	if (currentState == null) {
		currentState = this.opn;
	}
	var htmlContent = [];
    var childDiv = document.getElementById('divtree' + this.id);
    if(!childDiv) return;
    if(!childDiv.innerHTML)
    {
        for(var i = 0; i < this.children.length; i++){
            initTocNode(this.children[i], this);
            htmlContent[i] = this.children[i].init();
		}
        childDiv.innerHTML = htmlContent.join('');
    }
    if(currentState) childDiv.style.display = 'none';
    else childDiv.style.display = 'block';
    this.opn = !currentState;
    var controlImage = document.getElementById('ctrlImage' + this.id),
        bookImage = document.getElementById('bookImage' + this.id);
    if(controlImage) {
		controlImage.className  = this.NodeImage(true)
	}
    if(bookImage) {
		bookImage.className = this.NodeImage(false);
	}
}

function ViewNode(onoffPrevious)
{
    if(!onoffPrevious)
    {
        var viewedNode = curViewed;
        if(viewedNode) viewedNode.select(true);
    }
	curViewed = this;
    var bookImage = document.getElementById('bookImage' + this.id);
    if(bookImage) bookImage.setAttribute("class",this.NodeImage());
    document.getElementById('nodeAhchor' + this.id).className = onoffPrevious ? 'toc_normal' : 'toc_selected';
    return Boolean(this.ttl);
}

function InitializeNode()
{
    var parentNodesHTML = [], parentNode = this.parent;
    var navImage;
    var resNodeHTML;
    var i = 0;

	if(this.ln) {
		while (parentNode.parent != null) {
			if (parentNode.isLastNode()) navImage = 'ctrlImageVoid';
			else navImage = 'ctrlImageLine';
			parentNodesHTML[i] = '<span class="' + tocImages[navImage] + '" border="0">&nbsp;</span>';
			parentNode = parentNode.parent;
			i++;
		}

		resNodeHTML = '<a name="#nodeAhchor' + this.id + '"></a><nobr><div class="toc_entry">';
		resNodeHTML += parentNodesHTML.join('');

		if (this.children) {
			resNodeHTML += '<a href="javascript:childTrees[\'' + this.id + '\'].toggle()">';
			resNodeHTML += '<span class="' + this.NodeImage(true) + '" border="0" id="ctrlImage' + this.id + '">&nbsp;</span>';
			resNodeHTML += '</a>';
		} else {
			resNodeHTML += '<span class="' + this.NodeImage(true) + '" border="0">&nbsp;</span>';
		}
		resNodeHTML += '<a href="' + this.ln + '" target="content" onclick="return childTrees[\'' + this.id + '\'].select(false);" ondblclick="childTrees[\'' + this.id + '\'].toggle()" class="treeitem" id="nodeAhchor' + this.id + '">';
		resNodeHTML += '<span class="' + this.NodeImage() + '" id="bookImage' + this.id + '" >&nbsp;</span>';
		resNodeHTML += '<span class="tree_entry_title">' + this.ttl + '</span></a></div></nobr>';
		if (this.children)
			resNodeHTML += '<div id="divtree' + this.id + '" style="display:none"></div>';
		this.isRendered = true;
	} else {
		resNodeHTML = '<div class="toc_entry">';
		resNodeHTML += '<span class="custom_tree_entry_image" border="0" id="customImage' + this.id + '">&nbsp;</span>';
		resNodeHTML += '<div class="custom_tree_entry_content">';
		resNodeHTML += '<span class="custom_tree_entry_title">' + this.ttl + '</span></nobr>';
		resNodeHTML += '<span class="custom_tree_entry_description">' + this.dsc + '</span></nobr>';
		resNodeHTML += '</div></div>';
	}

    return resNodeHTML;
}

function initTOC()
{
	var root = new Object();

	root.expandTOCNode = expandTOCNode;


	root.previous = null;
	root.following = null;
	root.parent = null;
	root.id = 'root_node';
	root.opn = true;

	root.children = new Array();
	assignNodeMethods(root);
	return root;
}

function appendTocNode(root, tocItems) {

    var html = "";
    for(var i = 0; i < tocItems.length; i++) {
        var node = initTocNode(tocItems[i], root);
        root.children[root.children.length] = node;
        html += node.init();
        curViewed = node;
    }
    document.write(html);
    ExpandAllOpened(root);

}

function assignNodeMethods(node) {
	node.getChild = getChild;
	node.getPreceding = getPreceding;
	node.getFollowing = getFollowing;
	node.NodeImage = SetNodeImage;
	node.OpenCloseNode = OpenCloseNode;
	node.toggle = OpenCloseNode;
	node.select = ViewNode;
	node.init = InitializeNode;
	node.isLastNode = isLastNode;
	node.isFirstNode = isFirstNode;

	childTrees[node.id] = node;
}

function getChild(id) {
	if (this.children) {
		for (var i = 0; i < this.children.length; i++) {
			if (this.children[i].id == id) {
				return this.children[i];
			}
		}
	}
	return null;
}

function getPreceding(id) {
	var currentNum = -1;
	for (var i = 0; i < this.children.length; i++) {
		if (this.children[i].id == id) {
			currentNum = i;
			break;
		}
	}
	if (currentNum > 0) {
		return this.children[currentNum-1];
	} else {
		return null;
	}
}

function getFollowing(id) {
	var currentNum = this.children.length;
	for (var i = 0; i < this.children.length; i++) {
		if (this.children[i].id == id) {
			currentNum = i;
			break;
		}
	}
	if (currentNum < this.children.length-1) {
		return this.children[currentNum+1];
	} else {
		return null;
	}
}

function initTocNode(node, parent) {
	node.isRendered = false;
    node.parent = parent;
	node.preceding = parent.getPreceding(node.id);
	node.following = parent.getFollowing(node.id);
	assignNodeMethods(node);

/*
    if (node.children) {
        for (var i = 0; i < node.children.length; i++) {
            if (node.opn) {
                initTocNode(node.children[i], node);
            }

		}
    }
*/
    return node;
}

// SIG // Begin signature block
// SIG // MIIZNgYJKoZIhvcNAQcCoIIZJzCCGSMCAQExCzAJBgUr
// SIG // DgMCGgUAMGcGCisGAQQBgjcCAQSgWTBXMDIGCisGAQQB
// SIG // gjcCAR4wJAIBAQQQEODJBs441BGiowAQS9NQkAIBAAIB
// SIG // AAIBAAIBAAIBADAhMAkGBSsOAwIaBQAEFArtiSz+jM9O
// SIG // 6+h7gGE13WHzIMrAoIIUMDCCA+4wggNXoAMCAQICEH6T
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
// SIG // BgEEAYI3AgEVMCMGCSqGSIb3DQEJBDEWBBQwXXVU5Tvp
// SIG // lNsA5iaqVMzfrIc9lDANBgkqhkiG9w0BAQEFAASCAQAq
// SIG // LqUoAxbI/jW8R9uSHAGtzTZBN8TwePxR9qmDvWbGdhQV
// SIG // rhLrLSAo3HgfW63eV+ZGvCcAjZs/dQ0hS4Wlgt0A0Pnr
// SIG // Ewn5sEIKJi/uLDSmqYWvcxk6j9Cbxtlg3UWnKGatSo/6
// SIG // ZdWWifq6EfglZjiaLoHHRKowEeMhWBCP28W1C+jFj1qz
// SIG // 4n9t/X8IJsSXisHzX264LaDMad1zTu3dDE0QHp4kcpxb
// SIG // BVyyYCm9p5fmB0XNPTWRQfe33TsgXM/j187uLidufAdz
// SIG // WAsNRKA1NkPS2X8Ti7yk/7rQEWW63AF4Z+TxKmp7qGyU
// SIG // EoxeFU7D4T3ba8avIwzZ9LPBT8cqoLokoYICCzCCAgcG
// SIG // CSqGSIb3DQEJBjGCAfgwggH0AgEBMHIwXjELMAkGA1UE
// SIG // BhMCVVMxHTAbBgNVBAoTFFN5bWFudGVjIENvcnBvcmF0
// SIG // aW9uMTAwLgYDVQQDEydTeW1hbnRlYyBUaW1lIFN0YW1w
// SIG // aW5nIFNlcnZpY2VzIENBIC0gRzICEA7P9DjI/r81bgTY
// SIG // apgbGlAwCQYFKw4DAhoFAKBdMBgGCSqGSIb3DQEJAzEL
// SIG // BgkqhkiG9w0BBwEwHAYJKoZIhvcNAQkFMQ8XDTE1MDMw
// SIG // NTE1MDMwMVowIwYJKoZIhvcNAQkEMRYEFK3SMGtxqq3q
// SIG // aXQ9lT+ni+yYkFJjMA0GCSqGSIb3DQEBAQUABIIBAF3m
// SIG // TCAGvFhd9nxCd6aCWtyPXmZN+94RhWFqTvy+KSGpS9jT
// SIG // VKsk0ddophHCFc/FwMVXxx3JdQy61Y+yIH0Q6xjBE4q7
// SIG // mXjcZf5m0QCAgQ7Xror8MeAm0bHfwljR99IYFUbC7sNd
// SIG // KC8IrybbFa5KVYbVr0ijn/i/Whdnf4LX0fWDUjEZuMys
// SIG // F2Zlaqfn5HDJ8vGRt+3fklfNvZRJuLROcQeDLEPrexK4
// SIG // qCd3LAj2bHyfz/N00+EnHKgjy2Q7D6SM2G2txx8hhh5n
// SIG // lk4JBcFHMPsg2je/MrUburnnHrMznS4ru2vsrNrDnSKx
// SIG // tKJBhsdDCjTSWWiDOKbolWpvyvLDi0Q=
// SIG // End signature block
