<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:doc="http://schemas.atlassian.com/clover3/report" version="1.0" >
    <xsl:output method="html" version="1.0" encoding="utf-8" />
    <xsl:template match="/">
<html>
  <head>
    <title> Code Coverage Report </title>
    <style type="text/css" media="screen">
        body{ font-family:verdana;  font-size:12px}
        h1{ font-family:verdana;  font-size:25px;  text-shadow:1px 1px 1px #888}
        .reportTitle{ text-align:center}
        .rowHeader{ background:#ddedff;  border-bottom:#dcdcdc 1px solid;  border-top:#dcdcdc 1px solid;  border-right:#dcdcdc 1px solid;  font-size:14px;  padding:5px;  text-shadow:1px 0px 1px #888}
        .rowBody{ font-size:12px;  border-bottom:#9c9c9c 1px solid;  padding:10px}
        .projectRow{ font-size:14px !important;  font-weight:bold}
        .barBehind{ background: #FFF; border: 2px solid #CCC; border-radius: 5px; height: 20px; text-align: left; width: 100%; }
        .barFront{ background: #00DE00; border: 2px solid #00BD00; border-radius: 5px; height: 20px; left: -1px; position: relative; top: -2px; }
        .linebreak{ height:20px}
        td{ background:#fff}
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
        <xsl:variable name="covered" select="./doc:metrics/@coveredelements" />
        <xsl:variable name="total" select="./doc:metrics/@elements" />
        <xsl:variable name="notcovered" select="$total - $covered" />
        <xsl:variable name="percent" select="$covered div $total" />
        <tr>
          <td class="rowHeader"> </td>
          <td class="rowHeader">Files</td>
          <td class="rowHeader">Uncovered Elements</td>
          <td class="rowHeader">TOTAL Coverage</td>
          <td class="rowHeader"> </td>
        </tr>
        <tr>
            <td class="rowBody projectRow"><span class="title">Project</span></td>
            <td class="rowBody projectRow"><xsl:value-of select="count(.//doc:file)"/></td>
            <td class="rowBody projectRow"><xsl:value-of select="$notcovered"/></td>
            <td class="rowBody projectRow"><xsl:value-of select="format-number($percent, '0%')"/></td>
            <td class="rowBody" align="center">
                <div style="min-width: 200px; width: 100%" class="barBehind">
                    <div class="barFront">
                        <xsl:attribute name="style">width: <xsl:value-of select="format-number($percent, '0%')"/></xsl:attribute>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="linebreak" colspan="5" />
        </tr>
    </xsl:template>


    <xsl:template match="//doc:package">
        <xsl:variable name="covered" select="./doc:metrics/@coveredelements" />
        <xsl:variable name="total" select="./doc:metrics/@elements" />
        <xsl:variable name="notcovered" select="$total - $covered" />
        <xsl:variable name="percent" select="$covered div $total" />
        <tr>
            <td class="rowHeader"><span class="titleText"><xsl:value-of select="@name"/></span></td>
            <td class="rowHeader">Total Methods: <xsl:value-of select="count(.//doc:line[@type='method'])"/></td>
            <td class="rowHeader">Uncovered Elements: <xsl:value-of select="$notcovered"/></td>
            <td class="rowHeader">TOTAL Coverage: <xsl:value-of select="format-number($percent, '0%')"/></td>
            <td class="rowHeader" align="center">
                <div style="min-width: 200px;" class="barBehind ">
                    <div class="barFront ">
                        <xsl:attribute name="style">width: <xsl:value-of select="format-number($percent, '0%')"/></xsl:attribute>
                    </div>
                </div>
            </td>
        </tr>
        <xsl:apply-templates select=".//doc:file"/>
        <tr>
            <td class="linebreak" colspan="5" />
        </tr>
    </xsl:template>


    <xsl:template match="//doc:file">
        <xsl:variable name="covered" select="./doc:metrics/@coveredelements" />
        <xsl:variable name="total" select="./doc:metrics/@elements" />
        <xsl:variable name="notcovered" select="$total - $covered" />
        <xsl:variable name="percent" select="$covered div $total" />
        <tr>
            <td class="rowBody"><xsl:value-of select="@name"/></td>
            <td class="rowBody"><xsl:value-of select="count(.//doc:line[@type='method'])"/></td>
            <td class="rowBody"><xsl:value-of select="$notcovered"/></td>
            <td class="rowBody"><xsl:value-of select="format-number($percent, '0%')"/></td>
            <td class="rowBody" align="center">
                <div style="min-width: 200px;" class="barBehind ">
                    <div class="barFront ">
                        <xsl:attribute name="style">width: <xsl:value-of select="format-number($percent, '0%')"/></xsl:attribute>
                    </div>
                </div>
            </td>
        </tr>
    </xsl:template>


</xsl:stylesheet>