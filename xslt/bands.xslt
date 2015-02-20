<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:include href="page.xslt"/>



    <xsl:template match="band" mode="format">

        <p>
            <xsl:apply-templates select="description" mode="format" />
        </p>
        
        <xsl:if test="members">
            <p>
                <strong>
                    <xsl:value-of select="@id"/>
                </strong>
                <xsl:text> is...</xsl:text>
            </p>
            <ul>
                <xsl:for-each select="members/member">
                    <li>
                        <xsl:apply-templates mode="member" select="." />
                    </li>
                </xsl:for-each>
            </ul>

            <xsl:if test="friends">
                <p>
                    <xsl:text>...and is sometimes</xsl:text>
                </p>
                <ul>
                    <xsl:for-each select="friends/member">
                        <li>
                            <xsl:apply-templates mode="member" select="." />
                        </li>
                    </xsl:for-each>
                </ul>
            </xsl:if>

            <xsl:if test="formers">
                <p>
                    <xsl:text>...and was formerly</xsl:text>
                </p>
                <ul>
                    <xsl:for-each select="formers/member">
                        <li>
                            <xsl:apply-templates mode="member" select="." />
                        </li>
                    </xsl:for-each>
                </ul>
            </xsl:if>
        </xsl:if>
    </xsl:template>

    <xsl:template match="member" mode="member">
        <xsl:value-of select="@name" />
        <xsl:text> - </xsl:text>
        <xsl:value-of select="@role" />
        <xsl:if test="@sometimes | @formerly">
            <xsl:text> (</xsl:text>
            <xsl:if test="@sometimes">
                <xsl:text>sometimes </xsl:text>
                <xsl:value-of select="@sometimes" />
            </xsl:if>
            <xsl:if test="@sometimes and @formerly">
                <xsl:text>, </xsl:text>
            </xsl:if>
            <xsl:if test="@formerly">
                <xsl:text>formerly </xsl:text>
                <xsl:value-of select="@formerly" />
            </xsl:if>
            <xsl:text>)</xsl:text>
        </xsl:if>
    </xsl:template>


</xsl:stylesheet>
