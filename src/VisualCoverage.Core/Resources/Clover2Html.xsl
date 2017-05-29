<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:doc="http://schemas.atlassian.com/clover3/report" version="1.0" >
    <xsl:output method="html" version="1.0" encoding="utf-8" />
    <xsl:template match="/">
<html>
  <head>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.1.js">//</script>
    <link rel="stylesheet" type="text/css" href="https://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" />
    <script type="text/javascript" src="https://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js">//</script>
    <title> Code Coverage Report </title>
    <style type="text/css" media="screen">
        body { font-family:verdana;  font-size:12px; }
        h1 { font-family:verdana;  font-size:25px;  text-shadow:1px 1px 1px #888; }
        .reportTitle { text-align:center; }
        .rowHeader1 { font-size:14px; font-weight:bold; background:#def; border-bottom:#dcdcdc 1px solid; border-top:#dcdcdc 1px solid; border-right:#dcdcdc 1px solid; padding:5px; text-shadow:1px 0px 1px #888; }
        .rowHeader2 { font-size:12px; font-weight:bold; background:#eff; border-bottom:#9c9c9c 1px solid; padding:10px; text-shadow:1px 0px 1px #988; }
        .rowBody { font-size:12px; border-bottom:#9c9c9c 1px solid; padding:15px; }
        .projectRow { font-size:14px !important; font-weight:bold; }
        .barBehind { position:relative; background:#FFF; border:2px solid #CCC; border-radius:5px; height:24px; text-align:left; width:100%; }
        .barFront { position:absolute; background:#0D0; border:2px solid #0B0; border-radius:5px; height:24px; top:-2px; }
        .barLabel { position:relative; line-height:22px; font-weight:bold; width:100%; text-align:center; }
        .linebreak { height:20px; }
        td { background:#fff; }
        tr.clickable { cursor:pointer; }
        tr.clickable:hover { opacity:0.7; }
        tr.collapse.in { display:table-row; }
    </style>
  </head>
  <body>
    <h1 class="reportTitle">Coverage Report</h1>
    <table cellspacing="0" cellpadding="0" style="width:100%">
      <tbody>
        <xsl:apply-templates select="//doc:project"/>
        <xsl:apply-templates select="//doc:package"/>
      </tbody>
    </table>
  </body>
</html>
    </xsl:template>

    <xsl:template match="//doc:project">
        <xsl:variable name="lines" select="./doc:metrics/@lines" />
        <xsl:variable name="linescovered" select="./doc:metrics/@coveredlines" />
        <xsl:variable name="blocks" select="./doc:metrics/@blocks" />
        <xsl:variable name="blockscovered" select="./doc:metrics/@coveredblocks" />
        <xsl:variable name="blockspercent" select="$blockscovered div $blocks" />
        <tr>
          <td class="rowHeader1"> </td>
          <td class="rowHeader1">Covered Lines</td>
          <td class="rowHeader1">Covered Blocks</td>
          <td class="rowHeader1">Block Coverage</td>
        </tr>
        <tr>
            <td class="rowBody projectRow"><span class="title">Project</span></td>
            <td class="rowBody projectRow"><xsl:value-of select="$linescovered"/> / <xsl:value-of select="$lines"/></td>
            <td class="rowBody projectRow"><xsl:value-of select="$blockscovered"/> / <xsl:value-of select="$blocks"/></td>
            <td class="rowBody" align="center">
                <div style="min-width: 200px; width: 100%" class="barBehind">
                    <div class="barFront">
                      <xsl:attribute name="style">width: <xsl:value-of select="format-number($blockspercent, '0.0%')"/></xsl:attribute>
                    </div>
                </div>
                <div class="barLabel"><xsl:value-of select="format-number($blockspercent, '0.0%')"/></div>
            </td>
        </tr>
        <tr>
            <td class="linebreak" colspan="5" />
        </tr>
    </xsl:template>

    <xsl:template match="//doc:package">
        <xsl:variable name="lines" select="./doc:metrics/@lines" />
        <xsl:variable name="linescovered" select="./doc:metrics/@coveredlines" />
        <xsl:variable name="blocks" select="./doc:metrics/@blocks" />
        <xsl:variable name="blockscovered" select="./doc:metrics/@coveredblocks" />
        <xsl:variable name="blockspercent" select="$blockscovered div $blocks" />
        <xsl:variable name="packagename" select="translate(@name,'.','-')"/>

        <tr data-toggle="collapse" class="clickable">
            <xsl:attribute name="data-target">.package-<xsl:value-of select="$packagename"/></xsl:attribute>
            
            <td class="rowHeader1 projectRow"><span class="title"><xsl:value-of select="@name"/></span></td>
            <td class="rowHeader1 projectRow"><xsl:value-of select="$linescovered"/> / <xsl:value-of select="$lines"/></td>
            <td class="rowHeader1 projectRow"><xsl:value-of select="$blockscovered"/> / <xsl:value-of select="$blocks"/></td>
            <td class="rowHeader1" align="center">
                <div style="min-width: 200px; width: 100%" class="barBehind">
                    <div class="barFront">
                      <xsl:attribute name="style">width: <xsl:value-of select="format-number($blockspercent, '0.0%')"/></xsl:attribute>
                    </div>
                </div>
                <div class="barLabel"><xsl:value-of select="format-number($blockspercent, '0.0%')"/></div>
            </td>
        </tr>
        <xsl:apply-templates select=".//doc:class">
            <xsl:with-param name="packagename" select="$packagename"></xsl:with-param>
        </xsl:apply-templates>
        <tr>
            <td class="linebreak" colspan="5" />
        </tr>
    </xsl:template>

    <xsl:template match="//doc:class">
        <xsl:param name="packagename"/>
        <xsl:variable name="lines" select="./doc:metrics/@lines" />
        <xsl:variable name="linescovered" select="./doc:metrics/@coveredlines" />
        <xsl:variable name="blocks" select="./doc:metrics/@blocks" />
        <xsl:variable name="blockscovered" select="./doc:metrics/@coveredblocks" />
        <xsl:variable name="blockspercent" select="$blockscovered div $blocks" />
        <xsl:variable name="classname" select="translate(@name,'.','-')"/>

      
        <tr data-toggle="collapse">
            <xsl:attribute name="class">clickable package-<xsl:value-of select="$packagename"/> collapse</xsl:attribute>
            <xsl:attribute name="data-target">.method-<xsl:value-of select="$classname"/></xsl:attribute>
          
            <td class="rowHeader2"><xsl:value-of select="@name"/></td>
            <td class="rowHeader2"><xsl:value-of select="$linescovered"/> / <xsl:value-of select="$lines"/></td>
            <td class="rowHeader2"><xsl:value-of select="$blockscovered"/> / <xsl:value-of select="$blocks"/></td>
            <td class="rowHeader2" align="center">
                <div style="min-width: 200px; width: 100%" class="barBehind">
                    <div class="barFront">
                      <xsl:attribute name="style">width: <xsl:value-of select="format-number($blockspercent, '0.0%')"/></xsl:attribute>
                    </div>
                </div>
                <div class="barLabel"><xsl:value-of select="format-number($blockspercent, '0.0%')"/></div>
            </td>
        </tr>
        <xsl:apply-templates select=".//doc:method">
            <xsl:with-param name="classname" select="$classname"></xsl:with-param>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template match="//doc:method">
        <xsl:param name="classname"/>
        <xsl:variable name="lines" select="./doc:metrics/@lines" />
        <xsl:variable name="linescovered" select="./doc:metrics/@coveredlines" />
        <xsl:variable name="blocks" select="./doc:metrics/@blocks" />
        <xsl:variable name="blockscovered" select="./doc:metrics/@coveredblocks" />
        <xsl:variable name="blockspercent" select="$blockscovered div $blocks" />
        <tr>
            <xsl:attribute name="class">method-<xsl:value-of select="$classname"/> collapse</xsl:attribute>
          
            <td class="rowBody"><xsl:value-of select="@name"/></td>
            <td class="rowBody"><xsl:value-of select="$linescovered"/> / <xsl:value-of select="$lines"/></td>
            <td class="rowBody"><xsl:value-of select="$blockscovered"/> / <xsl:value-of select="$blocks"/></td>
            <td class="rowBody" align="center">
                <div style="min-width: 200px; width: 100%" class="barBehind">
                    <div class="barFront">
                      <xsl:attribute name="style">width: <xsl:value-of select="format-number($blockspercent, '0.0%')"/></xsl:attribute>
                    </div>
                </div>
                <div class="barLabel"><xsl:value-of select="format-number($blockspercent, '0.0%')"/></div>
            </td>
        </tr>
    </xsl:template>
</xsl:stylesheet>