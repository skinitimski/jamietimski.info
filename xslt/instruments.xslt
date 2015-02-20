<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:include href="page.xslt"/>



    <xsl:template match="instrument" mode="format">
        <xsl:element name="a">
            <xsl:attribute name="name">
                <xsl:value-of select="@id"/>
            </xsl:attribute>
        </xsl:element>
        <xsl:element name="table">
            <xsl:attribute name="class">section</xsl:attribute>
            <xsl:element name="tr">
                <xsl:element name="td">
                    <xsl:attribute name="width">65%</xsl:attribute>
                    <xsl:call-template name="field">
                        <xsl:with-param name="label" select="'Type: '" />
                        <xsl:with-param name="value" select="@type" />
                    </xsl:call-template>
                    <xsl:call-template name="field">
                        <xsl:with-param name="label" select="'Make: '" />
                        <xsl:with-param name="value" select="make" />
                    </xsl:call-template>
                    <xsl:call-template name="field">
                        <xsl:with-param name="label" select="'Model: '" />
                        <xsl:with-param name="value" select="model" />
                    </xsl:call-template>

                    <xsl:choose>

                        <xsl:when test="@type = 'Trombone'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Bore: '" />
                                <xsl:with-param name="value" select="bore" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Bell: '" />
                                <xsl:with-param name="value" select="bell" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Slide: '" />
                                <xsl:with-param name="value" select="slide" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Key: '" />
                                <xsl:with-param name="value" select="key" />
                            </xsl:call-template>
                        </xsl:when>

                        <xsl:when test="@type = 'Trumpet' or @type = 'Cornet' or @type = 'Flugelhorn'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Bore: '" />
                                <xsl:with-param name="value" select="bore" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Bell: '" />
                                <xsl:with-param name="value" select="bell" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Valves: '" />
                                <xsl:with-param name="value" select="valves" />
                            </xsl:call-template>
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Key: '" />
                                <xsl:with-param name="value" select="key" />
                            </xsl:call-template>
                        </xsl:when>

                        <xsl:when test="@type = 'Organ'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Keys: '" />
                                <xsl:with-param name="value" select="keys" />
                            </xsl:call-template>
                        </xsl:when>

                        <xsl:when test="@type = 'Electric Guitar' or @type = 'Ukulele' or @type = 'Acoustic Guitar'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Tuning: '" />
                                <xsl:with-param name="value" select="tuning" />
                            </xsl:call-template>
                        </xsl:when>

                        <xsl:when test="@type = 'Electric Guitar'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Pickups: '" />
                                <xsl:with-param name="value" select="pickups" />
                            </xsl:call-template>
                        </xsl:when>

                        <xsl:when test="@type = 'Bari'">
                            <xsl:call-template name="field">
                                <xsl:with-param name="label" select="'Low A? '" />
                                <xsl:with-param name="value" select="lowA" />
                            </xsl:call-template>
                        </xsl:when>

                    </xsl:choose>

                    <xsl:call-template name="field">
                        <xsl:with-param name="label" select="'Year: '" />
                        <xsl:with-param name="value" select="year" />
                    </xsl:call-template>
                    <xsl:call-template name="field">
                        <xsl:with-param name="label" select="'Cost: '" />
                        <xsl:with-param name="value" select="cost" />
                    </xsl:call-template>
                    <xsl:element name="br" />
                    <xsl:apply-templates mode="format" select="description" />
                </xsl:element>
                <xsl:element name="td">
                    <xsl:attribute name="width">35%</xsl:attribute>
                    <xsl:apply-templates mode="format" select="img" />
                </xsl:element>
            </xsl:element>
        </xsl:element>
    </xsl:template>


    <xsl:template name="field">
        <xsl:param name="label" />
        <xsl:param name="value" />
        <xsl:if test="string-length($value) > 0">
            <strong>
                <xsl:value-of select="$label" />
            </strong>
            <xsl:value-of select="$value" />
            <br />
        </xsl:if>
    </xsl:template>


</xsl:stylesheet>
