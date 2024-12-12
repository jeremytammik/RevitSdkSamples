<?xml version="1.0" encoding="gb2312"?>

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="frameName_left" select="'left_frame_null'" />
  <xsl:param name="frameName_main" select="'main_frame_null'" />

  <xsl:param name="frameName" select="''" />
  <xsl:variable name="url">xsl/</xsl:variable>

  <xsl:template match="/">
    <html>
      <head>
        <link rel="stylesheet" type="text/css" href="{$url}view.css"/>
      </head>
      <xsl:choose>
        <xsl:when test="$frameName = $frameName_left">
          <xsl:call-template name="left_frame_x" />
        </xsl:when>
        <xsl:when test="$frameName = $frameName_main">
          <xsl:call-template name="main_frame_x" />
        </xsl:when>
      </xsl:choose>

    </html>
  </xsl:template>

  <!--Left Frame here-->
  <xsl:template name="left_frame_x">
    <TABLE width="100%">
      <tr>
        <td colspan="2">
          --------<b>
            Diff (<xsl:value-of select="count(Types/Type[@compared='diff'])"/>)
          </b>--------
        </td>
      </tr>
      <xsl:apply-templates select="Types/Type[@compared='diff']" mode="index">
        <xsl:sort select="@namespace" order="ascending"  data-type="text"/>
        <xsl:sort select="@name" order="ascending" data-type="text"/>
      </xsl:apply-templates>
      <tr>
        <td colspan="2">
          --------<b>
            New (<xsl:value-of select="count(Types/Type[not(@compared)])"/>)
          </b>--------
        </td>
      </tr>
      <xsl:apply-templates select="Types/Type[not(@compared)]" mode="index">
        <xsl:sort select="@namespace" order="ascending"  data-type="text"/>
        <xsl:sort select="@name" order="ascending" data-type="text"/>
      </xsl:apply-templates>
    </TABLE>
  </xsl:template>

  <!--Main Frame here-->
  <xsl:template name="main_frame_x">
    <!--<xsl:apply-templates select="Types/Type" mode="content"/>-->
    <h1>Diff</h1>
    <xsl:apply-templates select="Types/Type[@compared='diff']" mode="content">
      <xsl:sort select="@namespace" order="ascending"  data-type="text"/>
      <xsl:sort select="@name" order="ascending" data-type="text"/>
    </xsl:apply-templates>
    <hr/>
    <h1>New</h1>
    <xsl:apply-templates select="Types/Type[not(@compared)]" mode="content">
      <xsl:sort select="@namespace" order="ascending"  data-type="text"/>
      <xsl:sort select="@name" order="ascending" data-type="text"/>
    </xsl:apply-templates>
  </xsl:template>

  <!--Index-->
  <xsl:template match="Type" mode="index">
    <tr>
      <td>
        <a href="javascript:parent.clickLink('{@fullname}');">
          <xsl:value-of select="@name"/>
        </a>
      </td>
      <td>
        <xsl:value-of select="@namespace"/>
      </td>
    </tr>
  </xsl:template>
  <!--Content-->
  <xsl:template match="Type" mode="content">
    <xsl:variable name="type" select="@type"/>
    <h3>
      <a name="{@fullname}">
        <xsl:value-of select="@name"/>
      </a>
    </h3>
    <small>
      <xsl:value-of select="@fullname"/> &#x21D2; <xsl:value-of select="@basetype"/>
    </small>
    <br/>
    <TABLE width="100%" class="content">
      <xsl:for-each select="*/*">
        <tr>
          <td width="20">
            <xsl:choose>
              <xsl:when test="$type='Enum' and name()='Field'">
                <img src="{concat($url,'VSObject_EnumItem')}.bmp"/>
              </xsl:when>
              <xsl:otherwise>
                <img src="{concat($url,'VSObject_',name())}.bmp"/>
              </xsl:otherwise>
            </xsl:choose>
          </td>
          <td width="20%">
            <xsl:value-of select="@name"/>
          </td>

          <td>
            <xsl:choose>
              <xsl:when test="name()='Field'">
                <xsl:value-of select="@type"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@text"/>
              </xsl:otherwise>
            </xsl:choose>
          </td>
        </tr>
      </xsl:for-each>
    </TABLE>
    <br/>
  </xsl:template>

</xsl:stylesheet>
