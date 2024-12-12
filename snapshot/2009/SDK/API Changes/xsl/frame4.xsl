<?xml version="1.0" encoding="gb2312"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" indent="yes" version="1.0"/>
  <xsl:template match="/">
    <html>
      <head>
        <SCRIPT LANGUAGE="JavaScript">
          <xsl:comment>
            <![CDATA[
var m_oXSL ; 
var m_sFrameAttr_left, m_sFrameAttr_main ;
var m_sXMLFile, m_sXSLTFile ;

m_sXMLFile = "" ;
m_sXSLTFile = "xsl/view4.xsl"; // 指定xsl文件,不要把/写成\了!
function window.onload()
{
   var oXSLDoc; 
   m_sFrameName_left = "left_frame"; 
   m_sFrameName_main = "main_frame";

   m_oXSL = new ActiveXObject("MSXML2.XSLTemplate.3.0");
   oXSLDoc = new ActiveXObject("MSXML2.FreeThreadedDOMDocument.3.0");

   oXSLDoc.async = false;
   oXSLDoc.load(m_sXSLTFile);
   m_oXSL.stylesheet= oXSLDoc;

   initPage() ;
}
function initPage()
{
   content.cols = "270,*" ;
   viewFrame(m_sFrameName_left);
   viewFrame(m_sFrameName_main);
}

function viewFrame(p_sFrameName)
{
   var oXSLProc;
   var sHtmlStr;

   oXSLProc  = m_oXSL.createProcessor();
   oXSLProc.input = xmlData;

   // 指定参数,显示左(或右)框架
   oXSLProc.addParameter("frameName_left", m_sFrameName_left);
   oXSLProc.addParameter("frameName_main", m_sFrameName_main);
   oXSLProc.addParameter("frameName", p_sFrameName);
   oXSLProc.transform();

   sHtmlStr = oXSLProc.output ; // 获得转化后的字符串
   //alert(sHtmlStr);
   eval(p_sFrameName + ".document").open ("text/html","UTF-8");
   eval(p_sFrameName + ".document").write(sHtmlStr) ;
   
   //var fso = new ActiveXObject("Scripting.FileSystemObject"); 
   // var a = fso.CreateTextFile("C:\\Documents and Settings\\sblu\\Desktop\\"+p_sFrameName+".html", true); 
   // a.Write(sHtmlStr); 
   // a.Close();
}

function clickLink(p_sDataID)
{
   eval(m_sFrameName_main + ".window").location.href="#"+p_sDataID;
}
            ]]>
          </xsl:comment>
        </SCRIPT>
      </head>
      <xml id="xmlData">
        <xsl:copy-of select="*"  />
      </xml>
      <frameset cols="0,*" name="content">
        <frame name="left_frame" src="about:blank" />
        <frame name="main_frame"  src="about:blank" />
      </frameset>
    </html>
  </xsl:template>

</xsl:stylesheet>

