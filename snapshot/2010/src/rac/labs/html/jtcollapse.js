//
// jtcollapse.css - collapse and expand certain sections of html file
//
// Copyright (C) 2007-2008 Jeremy Tammik, Autodesk Inc.
// All rights reserved.
// 2007-07-05
//
var jt_collapse_tag = 'pre';
var jt_collapse_class = 'jtcollapse';
var jt_title_attr = 'title';
var jt_state_attr = 'state';
var jt_collapsed_text_attr = 'collapsed_text';
var jt_expanded_text_attr = 'expanded_text';
var jt_expanded_text_prefix = '+ [';
var jt_expanded_text_suffix = '] (click to expand)';

//
// jt_escape - replace new line and spaces by HTML escape codes:
//
function jt_escape( s ) {
  // add a space into empty lines, so they do not get eliminated:
  var s1 = s.replace( /\r?\n\r?\n/g, '\n \n' );
  var s2 = s1
    .replace( /\/\/\/ \<\/?summary\>/gi, '//' )
    .replace( /\/\/\/ /g, '// ' )
    .replace( / /g, '&nbsp;' )
    .replace( /</g, '&lt;' )
    .replace( />/g, '&gt;' )
    .replace( /\n/g, '<br>' )
  //alert( s + '\n --> \n' + s1 + '\n --> \n' + s2 );
  return s2;
};

//
// getSelText - get the currently selected text in the browser window:
//
function getSelText()
{
  var txt = window.getSelection ? window.getSelection()
    : document.getSelection ? document.getSelection()
    : document.selection ? document.selection.createRange().text
    : '';
  return txt;
}

//
// jt_toggle - toggle collapsed and expanded state:
//
function jt_toggle() {
  if( '' == getSelText() ) {
    var state = this.getAttribute( jt_state_attr );
    state = !state;
    this.setAttribute( jt_state_attr, state );
    this.innerHTML = this.getAttribute( state ? jt_expanded_text_attr : jt_collapsed_text_attr );
  }
};

//
// jt_assignCollapse - set up collapsed and expanded state toggle:
//
function jt_assignCollapse( e, title )
{
  var body = e.innerHTML;
  e.setAttribute( jt_state_attr, 0 );
  e.setAttribute( jt_collapsed_text_attr, jt_escape( title ) );
  e.setAttribute( jt_expanded_text_attr, jt_escape( body ) );
  e.innerHTML = jt_expanded_text_prefix + title + jt_expanded_text_suffix;
  e.onclick = jt_toggle;
}

//
// jt_init - set up all collapsable <pre> tags:
//
function jt_init()
{
  if( document.getElementById && document.createTextNode ) {
    var entries = document.getElementsByTagName( jt_collapse_tag );
    for( i = 0; i < entries.length; ++i ) {
      var e = entries[i];
      if( jt_collapse_class == e.className ) {
        var title = e.getAttribute( jt_title_attr );
        if( title ) {
          jt_assignCollapse( e, title );
        }
        else {
          e.innerHTML = '<b>no title defined<b>';
        }
      }
    }
  }
}

window.onload = jt_init;
