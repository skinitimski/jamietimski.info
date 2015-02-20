<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
    version="1.0"
    xmlns:XsltExtensions="urn:XsltExtensions"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">


    <xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>



    <xsl:template match="/">

        <!--<xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>-->

        <xsl:element name="html">

            <xsl:element name="head">
                <xsl:element name="title">
                    <xsl:text>Timski</xsl:text>
                </xsl:element>
                <xsl:element name="link">
                    <xsl:attribute name="rel">stylesheet</xsl:attribute>
                    <xsl:attribute name="type">text/css</xsl:attribute>
                    <xsl:attribute name="href">style.css</xsl:attribute>
                </xsl:element>
                <xsl:element name="link">
                    <xsl:attribute name="rel">shortcut icon</xsl:attribute>
                    <xsl:attribute name="type">image/x-icon</xsl:attribute>
                    <xsl:attribute name="href">favicon.ico</xsl:attribute>
                </xsl:element>
                <xsl:element name="base">
                    <xsl:attribute name="target">_blank</xsl:attribute>
                </xsl:element>
                <xsl:element name="script">
                    <xsl:attribute name="type">text/javascript</xsl:attribute>
                    <xsl:attribute name="src">jquery-1.8.3.js</xsl:attribute>
                    <xsl:text> </xsl:text>
                </xsl:element>
            </xsl:element>

            <xsl:element name="body">

                <xsl:element name="table">

                    <xsl:attribute name="width">90%</xsl:attribute>

                    <!-- 9/4/2013 can't find usage anywhere -->
                    <xsl:if test="/page/@id">
                        <xsl:attribute name="id">
                            <xsl:value-of select="/page/@id" />
                        </xsl:attribute>
                    </xsl:if>


                    <xsl:element name="tr">
                        <xsl:attribute name="class">cell</xsl:attribute>
                        <xsl:element name="td">

                            <xsl:element name="a">
                                <xsl:attribute name="name">Top</xsl:attribute>
                            </xsl:element>

                            <xsl:apply-templates select="/page/node()" mode="format" />

                        </xsl:element>
                    </xsl:element>

                    <!-- Backlinks: Parent -->
                    <xsl:if test="not(/page[@suppressParentLink = 'true']) and /page/@parent">
                        <xsl:element name="tr">
                            <xsl:attribute name="class">cell</xsl:attribute>
                            <xsl:attribute name="style">text-align: center</xsl:attribute>
                            <xsl:element name="td">
                                <xsl:element name="strong">
                                    <xsl:call-template name="linkToParent" />
                                    <xsl:element name="br" />
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:if>

                    <!-- Backlinks: Home -->
                    <xsl:if test="not(/page[@suppressHomeLink = 'true'])">
                        <xsl:element name="tr">
                            <xsl:attribute name="class">cell</xsl:attribute>
                            <xsl:attribute name="style">text-align: center</xsl:attribute>
                            <xsl:element name="td">
                                <xsl:element name="strong">
                                    <xsl:call-template name="linkToHome" />
                                    <xsl:element name="br" />
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:if>

                    <!-- Timestamp -->
                    <xsl:call-template name="timestamp" />

                </xsl:element>
                
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />
                <xsl:element name="br" />                
                <xsl:element name="br" />
                
            </xsl:element>
        </xsl:element>
    </xsl:template>










    <!-- 
    Link to Parent
    -->
    <xsl:template name="linkToParent">
        <xsl:param name="text" select="'Back to Parent'" />
        <xsl:if test="/page/@parent">
            <xsl:element name="a">
                <xsl:attribute name="href">
                    <xsl:text>../</xsl:text>
                    <xsl:value-of select="/page/@parent" />
                    <xsl:text>.html</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="target">_self</xsl:attribute>
                <xsl:value-of select="$text" />
            </xsl:element>
            <xsl:element name="br" />
        </xsl:if>
    </xsl:template>


    <!--
    Link to Home
    -->
    <xsl:template name="linkToHome">
        <xsl:param name="text" select="'Back to Home'" />
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="XsltExtensions:PathToRoot()"/>
                <xsl:text>index.html</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="target">_self</xsl:attribute>
            <xsl:value-of select="$text" />
        </xsl:element>
        <xsl:element name="br" />
    </xsl:template>



    <!--
    timestamp
    -->
    <xsl:template name="timestamp">
        <xsl:element name="tr">
            <xsl:attribute name="class">cell</xsl:attribute>
            <xsl:attribute name="style">text-align: center</xsl:attribute>
            <xsl:element name="td">
                <xsl:element name="em">
                    <xsl:value-of select="XsltExtensions:Timestamp()" />
                </xsl:element>
            </xsl:element>
        </xsl:element>
    </xsl:template>











    <!--
    title template
    -->
    <xsl:template match="title" mode="format">
        <xsl:choose>
            <xsl:when test="@center = 'true'">
                <xsl:element name="table">
                    <xsl:attribute name="class">cell</xsl:attribute>
                    <xsl:element name="tr">
                        <xsl:attribute name="class">cell</xsl:attribute>
                        <xsl:element name="td">
                            <xsl:element name="h1">
                                <xsl:value-of select="."/>
                            </xsl:element>
                        </xsl:element>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:element name="h1">
                    <xsl:value-of select="."/>
                </xsl:element>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>



    <!--
    header template
    -->
    <xsl:template match="header | footer" mode="format">
        <xsl:choose>
            <xsl:when test="@center = 'true'">
                <xsl:element name="table">
                    <xsl:attribute name="class">cell</xsl:attribute>
                    <xsl:element name="tr">
                        <xsl:attribute name="class">cell</xsl:attribute>
                        <xsl:element name="td">
                            <xsl:apply-templates select="*" mode="format" />
                        </xsl:element>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="*" mode="format" />
            </xsl:otherwise>
        </xsl:choose>
        <xsl:element name="br" />
    </xsl:template>





    <!-- 
    sections template
    -->
    <xsl:template match="sections" mode="format">

        <xsl:if test="./*[@id]">

            <xsl:element name="h3">Index</xsl:element>

            <!-- Link to each section-->
            <xsl:for-each select="./*[@id and not(name() = 'hidden')]">
                <xsl:element name="a">
                    <xsl:attribute name="href">
                        <xsl:text>#</xsl:text>
                        <xsl:value-of select="@id" />
                    </xsl:attribute>
                    <xsl:attribute name="target">_self</xsl:attribute>
                    <xsl:value-of select="@id" />
                </xsl:element>
                <xsl:element name="br" />
            </xsl:for-each>

            <!-- Separate index from parent/home links-->
            <xsl:element name="br" />

            <xsl:if test="not(@suppressParentLink = 'true')">
                <xsl:call-template name="linkToParent" />
            </xsl:if>

            <xsl:if test="not(@suppressHomeLink = 'true')">
                <xsl:call-template name="linkToHome" />
            </xsl:if>

            <!-- Separate index from first content item on page -->
            <xsl:element name="br" />
            <xsl:element name="br" />

        </xsl:if>

        <xsl:for-each select="*[not(name() = 'hidden')]">
            <xsl:if test="@id">
                <xsl:element name="a">
                    <xsl:attribute name="name">
                        <xsl:value-of select="@id" />
                    </xsl:attribute>
                </xsl:element>
                <xsl:element name="h2">
                    <xsl:value-of select="@id" />
                </xsl:element>
            </xsl:if>
            <xsl:apply-templates select="." mode="format" />
            <xsl:if test="@id">
                <xsl:element name="a">
                    <xsl:attribute name="href">#Top</xsl:attribute>
                    <xsl:attribute name="target">_self</xsl:attribute>
                </xsl:element>
                <xsl:text>Back to Top</xsl:text>
                <xsl:element name="br" />
            </xsl:if>
            <xsl:element name="br" />
            <xsl:element name="br" />
        </xsl:for-each>

    </xsl:template>

    <!--
    Section template
    -->
    <xsl:template match="section" mode="format">
        <xsl:apply-templates select="*" mode="format" />
    </xsl:template>



    <!--
    index template
    -->
    <xsl:template match="index" mode="format">
        <xsl:variable name="subfolder" select="@subfolder" />
        <xsl:if test="@heading">
            <xsl:element name="h3">
                <xsl:value-of select="@heading"/>
            </xsl:element>
        </xsl:if>
        <xsl:for-each select="XsltExtensions:GetPages($subfolder)">
            <xsl:variable name="name" select="substring(.,0,string-length(.) - 3)" />
            <xsl:element name="a">
                <xsl:attribute name="href">
                    <xsl:value-of select="$subfolder" />
                    <xsl:text>/</xsl:text>
                    <xsl:value-of select="$name" />
                    <xsl:text>.html</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="target">_self</xsl:attribute>
                <xsl:value-of select="XsltExtensions:TitlizeWord($name)" />
            </xsl:element>
            <xsl:element name="br" />
        </xsl:for-each>
        <xsl:element name="br" />
    </xsl:template>


    <!--
    sidebar template
    -->
    <xsl:template match="sidebar" mode="format">

        <xsl:element name="table">

            <xsl:attribute name="class">
                <xsl:value-of select="name(.)" />
            </xsl:attribute>

            <xsl:element name="tr">

                <xsl:attribute name="class">
                    <xsl:value-of select="name(.)"/>
                </xsl:attribute>

                <xsl:element name="td">

                    <xsl:attribute name="class">
                        <xsl:value-of select="name(.)"/>
                    </xsl:attribute>

                    <xsl:attribute name="width">15%</xsl:attribute>
                    <xsl:attribute name="style">vertical-align: top; text-align: left;</xsl:attribute>

                    <xsl:element name="h3">
                        <xsl:choose>
                            <xsl:when test="@heading">
                                <xsl:value-of select="@heading" />
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:text>Pages</xsl:text>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:element>

                    <xsl:for-each select="XsltExtensions:GetSiblings()">
                        <xsl:variable name="name" select="substring(.,0,string-length(.) - 3)" />
                        <xsl:element name="a">
                            <xsl:attribute name="href">
                                <xsl:text>./</xsl:text>
                                <xsl:value-of select="$name" />
                                <xsl:text>.html</xsl:text>
                            </xsl:attribute>
                            <xsl:attribute name="target">_self</xsl:attribute>
                            <xsl:value-of select="XsltExtensions:TitlizeWord($name)" />
                        </xsl:element>
                        <xsl:element name="br" />
                    </xsl:for-each>

                    <xsl:element name="br" />

                    <xsl:call-template name="linkToParent">
                        <xsl:with-param name="text" select="'Back'" />
                    </xsl:call-template>

                </xsl:element>
                <xsl:element name="td">

                    <xsl:attribute name="class">
                        <xsl:value-of select="name(.)"/>
                    </xsl:attribute>

                    <xsl:if test="@center-content = 'true'">
                        <xsl:attribute name="style"></xsl:attribute>
                    </xsl:if>

                    <xsl:attribute name="width">85%</xsl:attribute>

                    <xsl:apply-templates mode="format" />

                </xsl:element>
            </xsl:element>
        </xsl:element>
    </xsl:template>




    <!--
    with-image template
    -->
    <xsl:template match="with-image" mode="format">
        <xsl:choose>
            <xsl:when test="image">
                <xsl:element name="table">
                    <xsl:attribute name="class">section</xsl:attribute>
                    <xsl:element name="tr">
                        <!-- Left Cell -->
                        <xsl:element name="td">
                            <xsl:choose>
                                <xsl:when test="@image-align = 'left'">
                                    <xsl:apply-templates select="image" mode="format" />
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:apply-templates select="text" mode="format" />
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:element>
                        <!-- Right Cell -->
                        <xsl:element name="td">
                            <xsl:choose>
                                <xsl:when test="@image-align = 'left'">
                                    <xsl:apply-templates select="text" mode="format" />
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:apply-templates select="image" mode="format" />
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:element>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="text" mode="format" />
            </xsl:otherwise>
        </xsl:choose>
        <xsl:element name="br" />
    </xsl:template>


    <!--
    values template
    -->
    <xsl:template match="values" mode="format">
        <xsl:for-each select="*">
            <xsl:element name="strong">
                <xsl:choose>
                    <xsl:when test="@label">
                        <xsl:value-of select="@label" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="XsltExtensions:TitlizeWord(name())" />
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:text>: </xsl:text>
            </xsl:element>
            <xsl:value-of select="." />
            <xsl:element name="br" />
        </xsl:for-each>
        <xsl:if test="not(@no-break-after = 'true' or @break-after = 'false')">
            <xsl:element name="br" />
        </xsl:if>
    </xsl:template>






    <!--
    quote template 
    -->
    <xsl:template match="quote" mode="format">
        <xsl:if test="@id">
            <xsl:element name="a">
                <xsl:attribute name="name">
                    <xsl:value-of select="@id"/>
                </xsl:attribute>
            </xsl:element>
        </xsl:if>
        <xsl:element name="table">
            <xsl:attribute name="class">section</xsl:attribute>
            <xsl:element name="tr">
                <xsl:element name="td">
                    <xsl:attribute name="width">60%</xsl:attribute>

                    <xsl:element name="table">
                        <xsl:attribute name="class">section</xsl:attribute>
                        <xsl:element name="tr">
                            <xsl:element name="td">
                                <xsl:element name="span">
                                    <xsl:attribute name="class">serif</xsl:attribute>
                                    <xsl:apply-templates mode="format" select="text" />
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:element>

                    <xsl:if test="credit">
                        <xsl:element name="table">
                            <xsl:attribute name="class">section</xsl:attribute>
                            <xsl:element name="tr">
                                <xsl:element name="td">
                                    <xsl:attribute name="width">60%</xsl:attribute>
                                </xsl:element>
                                <xsl:element name="td">
                                    <xsl:attribute name="width">40%</xsl:attribute>
                                    <xsl:element name="span">
                                        <xsl:element name="em">
                                            <xsl:text>-</xsl:text>
                                            <xsl:value-of select="credit/name" />
                                        </xsl:element>
                                        <xsl:if test="credit/birth">
                                            <xsl:text> (</xsl:text>
                                            <xsl:value-of select="credit/birth" />
                                            <xsl:text>-</xsl:text>
                                            <!-- may not exist, of course -->
                                            <xsl:value-of select="credit/death" />
                                            <xsl:text>)</xsl:text>
                                        </xsl:if>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:element>
                            <xsl:if test="credit/title">
                                <xsl:element name="tr">
                                    <xsl:element name="td" />
                                    <xsl:element name="td">
                                        <xsl:element name="span">
                                            <xsl:value-of select="credit/title" />
                                        </xsl:element>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:if>
                        </xsl:element>
                    </xsl:if>

                </xsl:element>

                <xsl:element name="td">
                    <xsl:attribute name="width">10%</xsl:attribute>
                </xsl:element>

                <xsl:element name="td">
                    <xsl:attribute name="width">30%</xsl:attribute>
                    <xsl:apply-templates mode="format" select="comments" />
                </xsl:element>

            </xsl:element>
            <!-- tr -->

        </xsl:element>
        <!-- table -->

        <xsl:element name="br" />
        <xsl:element name="br" />
    </xsl:template>


    <!--
    dialogue template
    -->
    <xsl:template match="dialogue" mode="format">
        <xsl:if test="@id">
            <xsl:element name="a">
                <xsl:attribute name="name">
                    <xsl:value-of select="@id"/>
                </xsl:attribute>
            </xsl:element>
        </xsl:if>
        <xsl:element name="table">
            <xsl:attribute name="class">section</xsl:attribute>
            <xsl:element name="tr">
                <xsl:element name="td">
                    <xsl:attribute name="width">60%</xsl:attribute>

                    <xsl:element name="table">
                        <xsl:attribute name="class">section</xsl:attribute>
                        <xsl:for-each select="line">
                            <xsl:element name="tr">
                                <xsl:element name="td">
                                    <xsl:element name="strong">
                                        <xsl:value-of select="player" />
                                        <xsl:text>: </xsl:text>
                                    </xsl:element>
                                    <xsl:element name="span">
                                        <xsl:attribute name="class">serif</xsl:attribute>
                                        <xsl:apply-templates mode="format" select="text" />
                                    </xsl:element>
                                </xsl:element>
                            </xsl:element>
                        </xsl:for-each>
                    </xsl:element>


                    <xsl:if test="credit">
                        <xsl:element name="table">
                            <xsl:attribute name="class">section</xsl:attribute>
                            <xsl:element name="tr">
                                <xsl:element name="td">
                                    <xsl:attribute name="width">60%</xsl:attribute>
                                </xsl:element>
                                <xsl:element name="td">
                                    <xsl:attribute name="width">40%</xsl:attribute>
                                    <xsl:element name="span">
                                        <xsl:element name="em">
                                            <xsl:text>From </xsl:text>
                                            <xsl:value-of select="credit" />
                                        </xsl:element>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:if>

                </xsl:element>

                <xsl:element name="td">
                    <xsl:attribute name="width">10%</xsl:attribute>
                </xsl:element>


                <xsl:element name="td">
                    <xsl:attribute name="width">30%</xsl:attribute>
                    <xsl:apply-templates mode="format" select="comments" />
                </xsl:element>

            </xsl:element>
            <!-- tr -->

        </xsl:element>
        <!-- table -->

        <xsl:element name="br" />
        <xsl:element name="br" />
    </xsl:template>



    <!--
    list template 
    -->
    <xsl:template match="list" mode="format">
        <xsl:apply-templates select="description" mode="format" />
        <xsl:element name="table">
            <xsl:attribute name="width">85%</xsl:attribute>
            <xsl:attribute name="border">1</xsl:attribute>
            <xsl:attribute name="cellpadding">5</xsl:attribute>


            <xsl:element name="tbody">
                <xsl:element name="tr">
                    <xsl:for-each select="columns/column">
                        <xsl:element name="th">
                            <xsl:if test="@width">
                                <xsl:attribute name="width">
                                    <xsl:value-of select="@width" />
                                </xsl:attribute>
                                <xsl:value-of select="." />
                            </xsl:if>
                        </xsl:element>
                    </xsl:for-each>
                </xsl:element>
                <xsl:for-each select="listItems/listItem">
                    <xsl:element name="tr">
                        <xsl:for-each select="columnValue">
                            <xsl:element name="td">
                                <xsl:attribute name="vAlign">
                                    <xsl:text>top</xsl:text>
                                </xsl:attribute>
                                <xsl:choose>
                                    <xsl:when test="position() = 1">
                                        <xsl:element name="strong">
                                            <xsl:apply-templates mode="format" select="." />
                                        </xsl:element>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:apply-templates mode="format" select="." />
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:element>
                        </xsl:for-each>
                    </xsl:element>
                </xsl:for-each>
            </xsl:element>
        </xsl:element>
    </xsl:template>


    <!--
    entry template
    -->
    <xsl:template match="entry" mode="format">
        <xsl:if test="@date">
            <xsl:element name="h3">
                <xsl:value-of select="@date" />
            </xsl:element>
        </xsl:if>
        <xsl:apply-templates mode="format" />
        <xsl:element name="p">
            <xsl:element name="em">
                <xsl:text>-</xsl:text>
                <xsl:choose>
                    <xsl:when test="@user">
                        <xsl:value-of select="@user" />
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text>timski</xsl:text>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:element>
        </xsl:element>
    </xsl:template>



    <!--
    cellTable template
    -->
    <xsl:template match="cellTable" mode="format">

        <xsl:element name="table">
            
            <xsl:attribute name="class">cell</xsl:attribute>

            <xsl:for-each select="@*">
                <xsl:attribute name="{name()}">
                    <xsl:value-of select="."/>
                </xsl:attribute>
            </xsl:for-each>

            <xsl:apply-templates mode="format" />            
            
        </xsl:element>
        
    </xsl:template>

    <xsl:template match="cellTable/tr" mode="format">

        <xsl:element name="tr">

            <xsl:attribute name="class">cell</xsl:attribute>

            <xsl:for-each select="@*">
                <xsl:attribute name="{name()}">
                    <xsl:value-of select="."/>
                </xsl:attribute>
            </xsl:for-each>

            <xsl:apply-templates mode="format" />

        </xsl:element>

    </xsl:template>



    <!--
    From-file template
    -->
    <xsl:template match="from-file" mode="format">
        <xsl:apply-templates select="XsltExtensions:InsertFileContent(@name)" mode="format" />
    </xsl:template>




    <!-- 
    Quoted template - activates link to quotes page for given quote
    -->
    <xsl:template match="quoted" mode="format">
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:text>quotes.html#</xsl:text>
                <xsl:value-of select="@id"/>
            </xsl:attribute>
            <xsl:attribute name="target">_self</xsl:attribute>
            <xsl:apply-templates mode="format" />
        </xsl:element>
    </xsl:template>


    <!--
    image template - uses optional caption with formatting
    -->
    <xsl:template match="image" mode="format">

        <xsl:element name="table">
            <xsl:attribute name="class">cell</xsl:attribute>
            <xsl:attribute name="style">width: 100%</xsl:attribute>
            <xsl:element name="tr">
                <xsl:attribute name="class">cell</xsl:attribute>
                <xsl:attribute name="style">width: 100%</xsl:attribute>
                <xsl:element name="td">
                    <xsl:element name="img">
                        <xsl:attribute name="src">
                            <xsl:value-of select="src" />
                        </xsl:attribute>
                        <xsl:if test="caption">
                            <xsl:attribute name="alt">
                                <xsl:value-of select="caption" />
                            </xsl:attribute>
                        </xsl:if>
                    </xsl:element>
                </xsl:element>
            </xsl:element>
            <xsl:if test="caption">
                <xsl:element name="tr">
                    <xsl:attribute name="class">cell</xsl:attribute>
                    <xsl:attribute name="style">width: 100%</xsl:attribute>
                    <xsl:element name="td">
                        <xsl:apply-templates select="caption" mode="format" />
                    </xsl:element>
                </xsl:element>
            </xsl:if>
        </xsl:element>

    </xsl:template>

    <!--
    Link template
    -->
    <xsl:template match="link" mode="format">

        <xsl:choose>

            <xsl:when test="@url">
                <xsl:apply-templates mode="format">
                    <xsl:with-param name="url" select="@url" />
                </xsl:apply-templates>
            </xsl:when>

            <xsl:otherwise>
                <xsl:if test="not(@no-space-before = 'true' or @space-before = 'false')">
                    <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:element name="a">
                    <xsl:if test="@class">
                        <xsl:attribute name="class">
                            <xsl:value-of select="@class"/>
                        </xsl:attribute>
                    </xsl:if>
                    <xsl:attribute name="href">
                        <xsl:value-of select="." />
                    </xsl:attribute>
                    <xsl:value-of select="." />
                </xsl:element>
                <xsl:if test="not(@no-space-after = 'true' or @space-after = 'false')">
                    <xsl:text> </xsl:text>
                </xsl:if>
            </xsl:otherwise>

        </xsl:choose>

    </xsl:template>


    <xsl:template match="link/wic" mode="format">
        <xsl:param name="url" />
        <xsl:if test="not(@no-space-before = 'true' or @space-before = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="$url" />
            </xsl:attribute>
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:if test="not(@no-space-after = 'true' or @space-after = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>


    <!--
    Hidden template - output nothing
    -->
    <xsl:template match="hidden" />
    <xsl:template match="hidden" mode="format" />



























    <xsl:template match="processing-instruction('php')" mode="format">
        <xsl:processing-instruction name="php">
            <xsl:value-of select="." />
        </xsl:processing-instruction>
    </xsl:template>
    
    
    
    
    
    
    
    
    


    <xsl:template match="font" mode="format">
        <span class="red">
            <xsl:text>FONT element found!!!</xsl:text>
        </span>
    </xsl:template>

    <!--
    Special handling for 'script'
    -->
    <xsl:template match="script" mode="format">
        <xsl:element name="{name()}">
            <xsl:for-each select="@*">
                <xsl:if test="not(name() = 'prefix' or name() = 'suffix')">
                    <xsl:attribute name="{name()}">
                        <xsl:value-of select="."/>
                    </xsl:attribute>
                </xsl:if>
            </xsl:for-each>
            <xsl:choose>
                <xsl:when test="@src">
                    <xsl:text> </xsl:text>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:apply-templates mode="format" />
                </xsl:otherwise>
            </xsl:choose>
        </xsl:element>
    </xsl:template>


    <!-- 
    Mapping: b -> strong
    -->
    <xsl:template match="b" mode="format">
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-before = 'true' or @space-before = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:element name="strong">
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-after = 'true' or @space-after = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>

    <!-- 
    Mapping: i -> em
    -->
    <xsl:template match="i" mode="format">
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-before = 'true' or @space-before = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:element name="em">
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-after = 'true' or @space-after = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>

    <!-- 
    Mapping: w -> span[@class="weak"]
    -->
    <xsl:template match="w" mode="format">
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-before = 'true' or @space-before = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:element name="span">
            <xsl:attribute name="class">weak</xsl:attribute>
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-after = 'true' or @space-after = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>

    <!-- 
    Mapping: l -> a[@target="_self"]
    -->
    <xsl:template match="l" mode="format">
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-before = 'true' or @space-before = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:element name="a">
            <xsl:attribute name="href">
                <xsl:value-of select="@href" />
            </xsl:attribute>
            <xsl:attribute name="target">_self</xsl:attribute>
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:if test="not(@spaces = 'false' or @no-spaces = 'true' or @no-space-after = 'true' or @space-after = 'false')">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>






    <!-- 
    As-is templates 
    -->
    <xsl:template
        mode="format"
        match="
        br |
        h1 | h2 | h3 |
        strong | em | 
        li | ul | ol | 
        p | span | 
        a | img | 
        table | tbody | tr | th | td |
        blockquote | q |
        iframe |
        form | input | label
        " >
        <xsl:value-of select="@prefix" />
        <xsl:element name="{name()}">
            <xsl:for-each select="@*">
                <xsl:if test="not(name() = 'prefix' or name() = 'suffix')">
                    <xsl:attribute name="{name()}">
                        <xsl:value-of select="."/>
                    </xsl:attribute>
                </xsl:if>
            </xsl:for-each>
            <xsl:apply-templates mode="format" />
        </xsl:element>
        <xsl:value-of select="@suffix" />
    </xsl:template>





    <!--
    Default templates
    -->

    <xsl:template match="text()" mode="format">
        <xsl:value-of select="normalize-space(.)"/>
    </xsl:template>

    <xsl:template match="text()">
        <xsl:value-of select="."/>
    </xsl:template>




</xsl:stylesheet>
