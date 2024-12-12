<?xml version="1.0" encoding="gb2312"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="frameName_left" select="'left_frame_null'" />
  <xsl:param name="frameName_main" select="'main_frame_null'" />

  <xsl:param name="frameName" select="''" />
  <xsl:variable name="url">xsl</xsl:variable>

  <xsl:template match="/">
    <html>
      <head>
        <link rel="stylesheet" type="text/css" href="{$url}/view.css"/>
      </head>

      <xsl:choose>
        <xsl:when test="$frameName = $frameName_left">
          <xsl:call-template name="left_frame" />
        </xsl:when>
        <xsl:when test="$frameName = $frameName_main">
          <xsl:call-template name="main_frame" />
        </xsl:when>
      </xsl:choose>

    </html>
  </xsl:template>

  <!--Left Frame here-->
  <xsl:template name="left_frame">
    <xsl:apply-templates select="APIDifferent" mode="index"/>
  </xsl:template>

  <!--Main Frame here-->
  <xsl:template name="main_frame">
    <xsl:apply-templates select="APIDifferent" mode="content"/>
  </xsl:template>

  <!--Index-->
  <xsl:template match="APIDifferent" mode="index">
    <xsl:for-each select="*">
      <!--AddClass (40)-->
      <xsl:value-of select="name()"/> (
      <xsl:value-of select="count(Class | Enum | */Class | */Enum)"/>
      )<br/>
      <!--Links-->
      <xsl:for-each select="Class | Enum | */Class | */Enum">
        <xsl:sort select="@Name"/>
        <a href="javascript:parent.clickLink('{@Name}');">
          <xsl:value-of select="@Name"/>
        </a>
        <br/>
      </xsl:for-each>
      <br/>
    </xsl:for-each>
  </xsl:template>

  <!--Content-->
  <xsl:template match="APIDifferent" mode="content">
    <!--Classes-->
    <xsl:for-each select="*">
      <h1>
        <xsl:value-of select="name()"/>
      </h1>
      <xsl:for-each select="Class | Enum | */Class | */Enum">
        <xsl:sort select="@Name"/>
        <h3>
          <a name="{@Name}">
            <xsl:value-of select="@Name"/>
          </a>
        </h3>
        <small>
          <xsl:value-of select="FullName"/>
        </small>
        <br/>
        <TABLE width="100%">
          <xsl:for-each select="*/*/* | Members/EnumName">
            <tr>
              <td width="20">
                <img src="{concat($url,'/',name())}.gif"/>
              </td>
              <td width="20%">
                <!--<xsl:if test="@name">
                  <xsl:value-of select="@Name | ."/>
                </xsl:if>-->
                <xsl:choose>
                  <xsl:when test="@Name">
                    <xsl:value-of select="@Name"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="."/>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td>
                <xsl:value-of select="Declare"/>
              </td>
            </tr>
          </xsl:for-each>
        </TABLE>
        <div align="right">
          <a href="#">Top</a>
        </div>
        
      </xsl:for-each>
      <br/>
      <hr/>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>

