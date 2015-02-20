<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:include href="page.xslt"/>

    <xsl:variable name="imgFolder" select="'gallery/physicists/'" />



    <xsl:template match="group">
        <xsl:apply-templates mode="physics" />
    </xsl:template>




    <xsl:template match="physicist" mode="physics">
        <!--
        <xsl:element name="a">
            <xsl:attribute name="name">
                <xsl:value-of select="@id"/>
            </xsl:attribute>
        </xsl:element>
        -->
        <table class="cell">
            <tr class="cell">
                <td>
                    <font class="normal">
                        <xsl:apply-templates mode="format" select="prose" />
                    </font>
                </td>
                <td></td>
                <td width="100px">
                    <xsl:variable name="img" select="img" />
                    <xsl:element name="img">
                        <xsl:attribute name="src">
                            <xsl:value-of select="concat($imgFolder, $img)"/>
                        </xsl:attribute>
                    </xsl:element>
                </td>
            </tr>
        </table>
    </xsl:template>




</xsl:stylesheet>
