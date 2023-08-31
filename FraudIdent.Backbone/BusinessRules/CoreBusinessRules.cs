using FraudIdent.Backbone.BusinessEntities;
using FraudIdent.Backbone.Providers;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FraudIdent.Backbone.BusinessRules
{
    public class CoreBusinessRules
    {
        private IConfiguration _config;

        private readonly MqttClientRequest mqttClient;
        CoreSQLProvider CoreSqlProvider { get; set; }

        string _topicsToSubscribe { get; set; }

        static bool alreadyCreateSubscribs { get; set; }


        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        {".323", "text/h323"},
        {".3g2", "video/3gpp2"},
        {".3gp", "video/3gpp"},
        {".3gp2", "video/3gpp2"},
        {".3gpp", "video/3gpp"},
        {".7z", "application/x-7z-compressed"},
        {".aa", "audio/audible"},
        {".AAC", "audio/aac"},
        {".aaf", "application/octet-stream"},
        {".aax", "audio/vnd.audible.aax"},
        {".ac3", "audio/ac3"},
        {".aca", "application/octet-stream"},
        {".accda", "application/msaccess.addin"},
        {".accdb", "application/msaccess"},
        {".accdc", "application/msaccess.cab"},
        {".accde", "application/msaccess"},
        {".accdr", "application/msaccess.runtime"},
        {".accdt", "application/msaccess"},
        {".accdw", "application/msaccess.webapplication"},
        {".accft", "application/msaccess.ftemplate"},
        {".acx", "application/internet-property-stream"},
        {".AddIn", "text/xml"},
        {".ade", "application/msaccess"},
        {".adobebridge", "application/x-bridge-url"},
        {".adp", "application/msaccess"},
        {".ADT", "audio/vnd.dlna.adts"},
        {".ADTS", "audio/aac"},
        {".afm", "application/octet-stream"},
        {".ai", "application/postscript"},
        {".aif", "audio/x-aiff"},
        {".aifc", "audio/aiff"},
        {".aiff", "audio/aiff"},
        {".air", "application/vnd.adobe.air-application-installer-package+zip"},
        {".amc", "application/x-mpeg"},
        {".application", "application/x-ms-application"},
        {".art", "image/x-jg"},
        {".asa", "application/xml"},
        {".asax", "application/xml"},
        {".ascx", "application/xml"},
        {".asd", "application/octet-stream"},
        {".asf", "video/x-ms-asf"},
        {".ashx", "application/xml"},
        {".asi", "application/octet-stream"},
        {".asm", "text/plain"},
        {".asmx", "application/xml"},
        {".aspx", "application/xml"},
        {".asr", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".atom", "application/atom+xml"},
        {".au", "audio/basic"},
        {".avi", "video/x-msvideo"},
        {".axs", "application/olescript"},
        {".bas", "text/plain"},
        {".bcpio", "application/x-bcpio"},
        {".bin", "application/octet-stream"},
        {".bmp", "image/bmp"},
        {".c", "text/plain"},
        {".cab", "application/octet-stream"},
        {".caf", "audio/x-caf"},
        {".calx", "application/vnd.ms-office.calx"},
        {".cat", "application/vnd.ms-pki.seccat"},
        {".cc", "text/plain"},
        {".cd", "text/plain"},
        {".cdda", "audio/aiff"},
        {".cdf", "application/x-cdf"},
        {".cer", "application/x-x509-ca-cert"},
        {".chm", "application/octet-stream"},
        {".class", "application/x-java-applet"},
        {".clp", "application/x-msclip"},
        {".cmx", "image/x-cmx"},
        {".cnf", "text/plain"},
        {".cod", "image/cis-cod"},
        {".config", "application/xml"},
        {".contact", "text/x-ms-contact"},
        {".coverage", "application/xml"},
        {".cpio", "application/x-cpio"},
        {".cpp", "text/plain"},
        {".crd", "application/x-mscardfile"},
        {".crl", "application/pkix-crl"},
        {".crt", "application/x-x509-ca-cert"},
        {".cs", "text/plain"},
        {".csdproj", "text/plain"},
        {".csh", "application/x-csh"},
        {".csproj", "text/plain"},
        {".css", "text/css"},
        {".csv", "text/csv"},
        {".cur", "application/octet-stream"},
        {".cxx", "text/plain"},
        {".dat", "application/octet-stream"},
        {".datasource", "application/xml"},
        {".dbproj", "text/plain"},
        {".dcr", "application/x-director"},
        {".def", "text/plain"},
        {".deploy", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dgml", "application/xml"},
        {".dib", "image/bmp"},
        {".dif", "video/x-dv"},
        {".dir", "application/x-director"},
        {".disco", "text/xml"},
        {".dll", "application/x-msdownload"},
        {".dll.config", "text/xml"},
        {".dlm", "text/dlm"},
        {".doc", "application/msword"},
        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
        {".dot", "application/msword"},
        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
        {".dsp", "application/octet-stream"},
        {".dsw", "text/plain"},
        {".dtd", "text/xml"},
        {".dtsConfig", "text/xml"},
        {".dv", "video/x-dv"},
        {".dvi", "application/x-dvi"},
        {".dwf", "drawing/x-dwf"},
        {".dwp", "application/octet-stream"},
        {".dxr", "application/x-director"},
        {".eml", "message/rfc822"},
        {".emz", "application/octet-stream"},
        {".eot", "application/octet-stream"},
        {".eps", "application/postscript"},
        {".etl", "application/etl"},
        {".etx", "text/x-setext"},
        {".evy", "application/envoy"},
        {".exe", "application/octet-stream"},
        {".exe.config", "text/xml"},
        {".fdf", "application/vnd.fdf"},
        {".fif", "application/fractals"},
        {".filters", "Application/xml"},
        {".fla", "application/octet-stream"},
        {".flr", "x-world/x-vrml"},
        {".flv", "video/x-flv"},
        {".fsscript", "application/fsharp-script"},
        {".fsx", "application/fsharp-script"},
        {".generictest", "application/xml"},
        {".gif", "image/gif"},
        {".group", "text/x-ms-group"},
        {".gsm", "audio/x-gsm"},
        {".gtar", "application/x-gtar"},
        {".gz", "application/x-gzip"},
        {".h", "text/plain"},
        {".hdf", "application/x-hdf"},
        {".hdml", "text/x-hdml"},
        {".hhc", "application/x-oleobject"},
        {".hhk", "application/octet-stream"},
        {".hhp", "application/octet-stream"},
        {".hlp", "application/winhlp"},
        {".hpp", "text/plain"},
        {".hqx", "application/mac-binhex40"},
        {".hta", "application/hta"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".htt", "text/webviewhtml"},
        {".hxa", "application/xml"},
        {".hxc", "application/xml"},
        {".hxd", "application/octet-stream"},
        {".hxe", "application/xml"},
        {".hxf", "application/xml"},
        {".hxh", "application/octet-stream"},
        {".hxi", "application/octet-stream"},
        {".hxk", "application/xml"},
        {".hxq", "application/octet-stream"},
        {".hxr", "application/octet-stream"},
        {".hxs", "application/octet-stream"},
        {".hxt", "text/html"},
        {".hxv", "application/xml"},
        {".hxw", "application/octet-stream"},
        {".hxx", "text/plain"},
        {".i", "text/plain"},
        {".ico", "image/x-icon"},
        {".ics", "application/octet-stream"},
        {".idl", "text/plain"},
        {".ief", "image/ief"},
        {".iii", "application/x-iphone"},
        {".inc", "text/plain"},
        {".inf", "application/octet-stream"},
        {".inl", "text/plain"},
        {".ins", "application/x-internet-signup"},
        {".ipa", "application/x-itunes-ipa"},
        {".ipg", "application/x-itunes-ipg"},
        {".ipproj", "text/plain"},
        {".ipsw", "application/x-itunes-ipsw"},
        {".iqy", "text/x-ms-iqy"},
        {".isp", "application/x-internet-signup"},
        {".ite", "application/x-itunes-ite"},
        {".itlp", "application/x-itunes-itlp"},
        {".itms", "application/x-itunes-itms"},
        {".itpc", "application/x-itunes-itpc"},
        {".IVF", "video/x-ivf"},
        {".jar", "application/java-archive"},
        {".java", "application/octet-stream"},
        {".jck", "application/liquidmotion"},
        {".jcz", "application/liquidmotion"},
        {".jfif", "image/pjpeg"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpb", "application/octet-stream"},
        {".jpe", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".json", "application/json"},
        {".jsx", "text/jscript"},
        {".jsxbin", "text/plain"},
        {".latex", "application/x-latex"},
        {".library-ms", "application/windows-library+xml"},
        {".lit", "application/x-ms-reader"},
        {".loadtest", "application/xml"},
        {".lpk", "application/octet-stream"},
        {".lsf", "video/x-la-asf"},
        {".lst", "text/plain"},
        {".lsx", "video/x-la-asf"},
        {".lzh", "application/octet-stream"},
        {".m13", "application/x-msmediaview"},
        {".m14", "application/x-msmediaview"},
        {".m1v", "video/mpeg"},
        {".m2t", "video/vnd.dlna.mpeg-tts"},
        {".m2ts", "video/vnd.dlna.mpeg-tts"},
        {".m2v", "video/mpeg"},
        {".m3u", "audio/x-mpegurl"},
        {".m3u8", "audio/x-mpegurl"},
        {".m4a", "audio/m4a"},
        {".m4b", "audio/m4b"},
        {".m4p", "audio/m4p"},
        {".m4r", "audio/x-m4r"},
        {".m4v", "video/x-m4v"},
        {".mac", "image/x-macpaint"},
        {".mak", "text/plain"},
        {".man", "application/x-troff-man"},
        {".manifest", "application/x-ms-manifest"},
        {".map", "text/plain"},
        {".master", "application/xml"},
        {".mda", "application/msaccess"},
        {".mdb", "application/x-msaccess"},
        {".mde", "application/msaccess"},
        {".mdp", "application/octet-stream"},
        {".me", "application/x-troff-me"},
        {".mfp", "application/x-shockwave-flash"},
        {".mht", "message/rfc822"},
        {".mhtml", "message/rfc822"},
        {".mid", "audio/mid"},
        {".midi", "audio/mid"},
        {".mix", "application/octet-stream"},
        {".mk", "text/plain"},
        {".mmf", "application/x-smaf"},
        {".mno", "text/xml"},
        {".mny", "application/x-msmoney"},
        {".mod", "video/mpeg"},
        {".mov", "video/quicktime"},
        {".movie", "video/x-sgi-movie"},
        {".mp2", "video/mpeg"},
        {".mp2v", "video/mpeg"},
        {".mp3", "audio/mpeg"},
        {".mp4", "video/mp4"},
        {".mp4v", "video/mp4"},
        {".mpa", "video/mpeg"},
        {".mpe", "video/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpf", "application/vnd.ms-mediapackage"},
        {".mpg", "video/mpeg"},
        {".mpp", "application/vnd.ms-project"},
        {".mpv2", "video/mpeg"},
        {".mqv", "video/quicktime"},
        {".ms", "application/x-troff-ms"},
        {".msi", "application/octet-stream"},
        {".mso", "application/octet-stream"},
        {".mts", "video/vnd.dlna.mpeg-tts"},
        {".mtx", "application/xml"},
        {".mvb", "application/x-msmediaview"},
        {".mvc", "application/x-miva-compiled"},
        {".mxp", "application/x-mmxp"},
        {".nc", "application/x-netcdf"},
        {".nsc", "video/x-ms-asf"},
        {".nws", "message/rfc822"},
        {".ocx", "application/octet-stream"},
        {".oda", "application/oda"},
        {".odc", "text/x-ms-odc"},
        {".odh", "text/plain"},
        {".odl", "text/plain"},
        {".odp", "application/vnd.oasis.opendocument.presentation"},
        {".ods", "application/oleobject"},
        {".odt", "application/vnd.oasis.opendocument.text"},
        {".one", "application/onenote"},
        {".onea", "application/onenote"},
        {".onepkg", "application/onenote"},
        {".onetmp", "application/onenote"},
        {".onetoc", "application/onenote"},
        {".onetoc2", "application/onenote"},
        {".orderedtest", "application/xml"},
        {".osdx", "application/opensearchdescription+xml"},
        {".p10", "application/pkcs10"},
        {".p12", "application/x-pkcs12"},
        {".p7b", "application/x-pkcs7-certificates"},
        {".p7c", "application/pkcs7-mime"},
        {".p7m", "application/pkcs7-mime"},
        {".p7r", "application/x-pkcs7-certreqresp"},
        {".p7s", "application/pkcs7-signature"},
        {".pbm", "image/x-portable-bitmap"},
        {".pcast", "application/x-podcast"},
        {".pct", "image/pict"},
        {".pcx", "application/octet-stream"},
        {".pcz", "application/octet-stream"},
        {".pdf", "application/pdf"},
        {".pfb", "application/octet-stream"},
        {".pfm", "application/octet-stream"},
        {".pfx", "application/x-pkcs12"},
        {".pgm", "image/x-portable-graymap"},
        {".pic", "image/pict"},
        {".pict", "image/pict"},
        {".pkgdef", "text/plain"},
        {".pkgundef", "text/plain"},
        {".pko", "application/vnd.ms-pki.pko"},
        {".pls", "audio/scpls"},
        {".pma", "application/x-perfmon"},
        {".pmc", "application/x-perfmon"},
        {".pml", "application/x-perfmon"},
        {".pmr", "application/x-perfmon"},
        {".pmw", "application/x-perfmon"},
        {".png", "image/png"},
        {".pnm", "image/x-portable-anymap"},
        {".pnt", "image/x-macpaint"},
        {".pntg", "image/x-macpaint"},
        {".pot", "application/vnd.ms-powerpoint"},
        {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
        {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
        {".ppa", "application/vnd.ms-powerpoint"},
        {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
        {".ppm", "image/x-portable-pixmap"},
        {".pps", "application/vnd.ms-powerpoint"},
        {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
        {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
        {".ppt", "application/vnd.ms-powerpoint"},
        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
        {".prf", "application/pics-rules"},
        {".prm", "application/octet-stream"},
        {".prx", "application/octet-stream"},
        {".ps", "application/postscript"},
        {".psc1", "application/PowerShell"},
        {".psd", "application/octet-stream"},
        {".psess", "application/xml"},
        {".psm", "application/octet-stream"},
        {".psp", "application/octet-stream"},
        {".pub", "application/x-mspublisher"},
        {".pwz", "application/vnd.ms-powerpoint"},
        {".qht", "text/x-html-insertion"},
        {".qhtm", "text/x-html-insertion"},
        {".qt", "video/quicktime"},
        {".qti", "image/x-quicktime"},
        {".qtif", "image/x-quicktime"},
        {".qtl", "application/x-quicktimeplayer"},
        {".qxd", "application/octet-stream"},
        {".ra", "audio/x-pn-realaudio"},
        {".ram", "audio/x-pn-realaudio"},
        {".rar", "application/octet-stream"},
        {".ras", "image/x-cmu-raster"},
        {".rat", "application/rat-file"},
        {".rc", "text/plain"},
        {".rc2", "text/plain"},
        {".rct", "text/plain"},
        {".rdlc", "application/xml"},
        {".resx", "application/xml"},
        {".rf", "image/vnd.rn-realflash"},
        {".rgb", "image/x-rgb"},
        {".rgs", "text/plain"},
        {".rm", "application/vnd.rn-realmedia"},
        {".rmi", "audio/mid"},
        {".rmp", "application/vnd.rn-rn_music_package"},
        {".roff", "application/x-troff"},
        {".rpm", "audio/x-pn-realaudio-plugin"},
        {".rqy", "text/x-ms-rqy"},
        {".rtf", "application/rtf"},
        {".rtx", "text/richtext"},
        {".ruleset", "application/xml"},
        {".s", "text/plain"},
        {".safariextz", "application/x-safari-safariextz"},
        {".scd", "application/x-msschedule"},
        {".sct", "text/scriptlet"},
        {".sd2", "audio/x-sd2"},
        {".sdp", "application/sdp"},
        {".sea", "application/octet-stream"},
        {".searchConnector-ms", "application/windows-search-connector+xml"},
        {".setpay", "application/set-payment-initiation"},
        {".setreg", "application/set-registration-initiation"},
        {".settings", "application/xml"},
        {".sgimb", "application/x-sgimb"},
        {".sgml", "text/sgml"},
        {".sh", "application/x-sh"},
        {".shar", "application/x-shar"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".sitemap", "application/xml"},
        {".skin", "application/xml"},
        {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
        {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
        {".slk", "application/vnd.ms-excel"},
        {".sln", "text/plain"},
        {".slupkg-ms", "application/x-ms-license"},
        {".smd", "audio/x-smd"},
        {".smi", "application/octet-stream"},
        {".smx", "audio/x-smd"},
        {".smz", "audio/x-smd"},
        {".snd", "audio/basic"},
        {".snippet", "application/xml"},
        {".snp", "application/octet-stream"},
        {".sol", "text/plain"},
        {".sor", "text/plain"},
        {".spc", "application/x-pkcs7-certificates"},
        {".spl", "application/futuresplash"},
        {".src", "application/x-wais-source"},
        {".srf", "text/plain"},
        {".SSISDeploymentManifest", "text/xml"},
        {".ssm", "application/streamingmedia"},
        {".sst", "application/vnd.ms-pki.certstore"},
        {".stl", "application/vnd.ms-pki.stl"},
        {".sv4cpio", "application/x-sv4cpio"},
        {".sv4crc", "application/x-sv4crc"},
        {".svc", "application/xml"},
        {".swf", "application/x-shockwave-flash"},
        {".t", "application/x-troff"},
        {".tar", "application/x-tar"},
        {".tcl", "application/x-tcl"},
        {".testrunconfig", "application/xml"},
        {".testsettings", "application/xml"},
        {".tex", "application/x-tex"},
        {".texi", "application/x-texinfo"},
        {".texinfo", "application/x-texinfo"},
        {".tgz", "application/x-compressed"},
        {".thmx", "application/vnd.ms-officetheme"},
        {".thn", "application/octet-stream"},
        {".tif", "image/tiff"},
        {".tiff", "image/tiff"},
        {".tlh", "text/plain"},
        {".tli", "text/plain"},
        {".toc", "application/octet-stream"},
        {".tr", "application/x-troff"},
        {".trm", "application/x-msterminal"},
        {".trx", "application/xml"},
        {".ts", "video/vnd.dlna.mpeg-tts"},
        {".tsv", "text/tab-separated-values"},
        {".ttf", "application/octet-stream"},
        {".tts", "video/vnd.dlna.mpeg-tts"},
        {".txt", "text/plain"},
        {".u32", "application/octet-stream"},
        {".uls", "text/iuls"},
        {".user", "text/plain"},
        {".ustar", "application/x-ustar"},
        {".vb", "text/plain"},
        {".vbdproj", "text/plain"},
        {".vbk", "video/mpeg"},
        {".vbproj", "text/plain"},
        {".vbs", "text/vbscript"},
        {".vcf", "text/x-vcard"},
        {".vcproj", "Application/xml"},
        {".vcs", "text/plain"},
        {".vcxproj", "Application/xml"},
        {".vddproj", "text/plain"},
        {".vdp", "text/plain"},
        {".vdproj", "text/plain"},
        {".vdx", "application/vnd.ms-visio.viewer"},
        {".vml", "text/xml"},
        {".vscontent", "application/xml"},
        {".vsct", "text/xml"},
        {".vsd", "application/vnd.visio"},
        {".vsi", "application/ms-vsi"},
        {".vsix", "application/vsix"},
        {".vsixlangpack", "text/xml"},
        {".vsixmanifest", "text/xml"},
        {".vsmdi", "application/xml"},
        {".vspscc", "text/plain"},
        {".vss", "application/vnd.visio"},
        {".vsscc", "text/plain"},
        {".vssettings", "text/xml"},
        {".vssscc", "text/plain"},
        {".vst", "application/vnd.visio"},
        {".vstemplate", "text/xml"},
        {".vsto", "application/x-ms-vsto"},
        {".vsw", "application/vnd.visio"},
        {".vsx", "application/vnd.visio"},
        {".vtx", "application/vnd.visio"},
        {".wav", "audio/wav"},
        {".wave", "audio/wav"},
        {".wax", "audio/x-ms-wax"},
        {".wbk", "application/msword"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wcm", "application/vnd.ms-works"},
        {".wdb", "application/vnd.ms-works"},
        {".wdp", "image/vnd.ms-photo"},
        {".webarchive", "application/x-safari-webarchive"},
        {".webtest", "application/xml"},
        {".wiq", "application/xml"},
        {".wiz", "application/msword"},
        {".wks", "application/vnd.ms-works"},
        {".WLMP", "application/wlmoviemaker"},
        {".wlpginstall", "application/x-wlpg-detect"},
        {".wlpginstall3", "application/x-wlpg3-detect"},
        {".wm", "video/x-ms-wm"},
        {".wma", "audio/x-ms-wma"},
        {".wmd", "application/x-ms-wmd"},
        {".wmf", "application/x-msmetafile"},
        {".wml", "text/vnd.wap.wml"},
        {".wmlc", "application/vnd.wap.wmlc"},
        {".wmls", "text/vnd.wap.wmlscript"},
        {".wmlsc", "application/vnd.wap.wmlscriptc"},
        {".wmp", "video/x-ms-wmp"},
        {".wmv", "video/x-ms-wmv"},
        {".wmx", "video/x-ms-wmx"},
        {".wmz", "application/x-ms-wmz"},
        {".wpl", "application/vnd.ms-wpl"},
        {".wps", "application/vnd.ms-works"},
        {".wri", "application/x-mswrite"},
        {".wrl", "x-world/x-vrml"},
        {".wrz", "x-world/x-vrml"},
        {".wsc", "text/scriptlet"},
        {".wsdl", "text/xml"},
        {".wvx", "video/x-ms-wvx"},
        {".x", "application/directx"},
        {".xaf", "x-world/x-vrml"},
        {".xaml", "application/xaml+xml"},
        {".xap", "application/x-silverlight-app"},
        {".xbap", "application/x-ms-xbap"},
        {".xbm", "image/x-xbitmap"},
        {".xdr", "text/plain"},
        {".xht", "application/xhtml+xml"},
        {".xhtml", "application/xhtml+xml"},
        {".xla", "application/vnd.ms-excel"},
        {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
        {".xlc", "application/vnd.ms-excel"},
        {".xld", "application/vnd.ms-excel"},
        {".xlk", "application/vnd.ms-excel"},
        {".xll", "application/vnd.ms-excel"},
        {".xlm", "application/vnd.ms-excel"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".xlt", "application/vnd.ms-excel"},
        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
        {".xlw", "application/vnd.ms-excel"},
        {".xml", "text/xml"},
        {".xmta", "application/xml"},
        {".xof", "x-world/x-vrml"},
        {".XOML", "text/plain"},
        {".xpm", "image/x-xpixmap"},
        {".xps", "application/vnd.ms-xpsdocument"},
        {".xrm-ms", "text/xml"},
        {".xsc", "application/xml"},
        {".xsd", "text/xml"},
        {".xsf", "text/xml"},
        {".xsl", "text/xml"},
        {".xslt", "text/xml"},
        {".xsn", "application/octet-stream"},
        {".xss", "application/xml"},
        {".xtp", "application/octet-stream"},
        {".xwd", "image/x-xwindowdump"},
        {".z", "application/x-compress"},
        {".zip", "application/x-zip-compressed"},
        #endregion
        
        };

        private static IDictionary<string, string> _mappingsMime = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        {"IVBOR", ".png"},
        {"/VBOR", ".jpeg"},
        {"/9J/4", ".jpeg"},
        {"JVBER", ".pdf"},
        {"R0lGODdh", ".gif"},
        {"uesdb", ".docx"},
        { "0m8r4k", ".doc"}
        #endregion
        
        };

        public CoreBusinessRules(IConfiguration _configuuration,
                                 CoreSQLProvider _CoreSqlProvider,
                                 MqttClientRequest _mqttClient,
                                 bool? onlyReference = false)
        {
            _config = _configuuration;
            this.CoreSqlProvider = _CoreSqlProvider;
            mqttClient = _mqttClient;

            if (onlyReference != true && !alreadyCreateSubscribs)
            {
                _topicsToSubscribe = _config.GetSection("MQTTConfig")["TopicsSubscribe"].ToString();

                SubscribeInTopics();
                alreadyCreateSubscribs = true;
            }
        }

        #region MQTT

        public async void SubscribeInTopics()
        {
            List<String> topics = _topicsToSubscribe.Split(";").ToList();

            // Connect to the MQTT broker
            var client = mqttClient.Connect();

            foreach (var topic in topics)
            {
                // Subscribe to a topic
                mqttClient.Subscribe(topic);
            }

            client.ApplicationMessageReceivedAsync += HandleMessageAsync;
        }

        async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            var _payLoad = e.ApplicationMessage.Payload;
            var payload = _payLoad != null ? Encoding.UTF8.GetString(_payLoad) : "";
            var topic = e.ApplicationMessage.Topic;

            var functionCallBackRequest = _config.GetSection("MQTTConfig")[topic];

            var _coreRules = new CoreBusinessRules(_config, this.CoreSqlProvider, mqttClient, true);

            MethodInfo methodInfo = typeof(CoreBusinessRules).GetMethod(functionCallBackRequest);

            // Create the parameter
            object[] parameters = new object[] { topic, payload };

            // Invoke the function
            try
            {
                methodInfo.Invoke(_coreRules, parameters);
            }
            catch (Exception ex)
            {
                mqttClient.Publish("applicationErrorMQtt", JsonConvert.SerializeObject(new { 
                    Topic = topic,
                    Message = payload,
                    Error = ex.ToString()
                }));
            }
        }

        #region MQTT Subscribe/Reponse
        public void GetBalanceInfoMQtt(string topic, string messag)
        {
            var balanceInfo = this.GetBalanceInfo();

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(balanceInfo));
        }

        public void SaveBalanceInfoMQtt(string topic, string messag)
        {
            var balanceInfo = JsonConvert.DeserializeObject<BalanceInfo>(messag);

            this.SaveBalanceInfo(balanceInfo);

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(balanceInfo));
        }
        public void GetTrucksMQtt(string topic, string messag)
        {
            List<Truck> list = new List<Truck>();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var truckFilter = JsonConvert.DeserializeObject<Truck>(messag);

                list = this.GetTrucks(truckFilter);
            }
            else
            {
                list = this.GetTrucks();
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(list));
        }

        public void SaveTrucksMQtt(string topic, string messag)
        {
            var truck = JsonConvert.DeserializeObject<Truck>(messag);

            this.SaveTruck(truck);
            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(truck));
        }

        public void GetTruckParamMQtt(string topic, string messag)
        {
            TruckParam entity = new TruckParam();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var truckParamId = JsonConvert.DeserializeObject<int>(messag);

                entity = this.GetTruckParam(truckParamId);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Informe o id do caminhão ex: 'TruckId' = 1");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void SaveTruckParamMQtt(string topic, string messag)
        {
            TruckParam entity = new TruckParam();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                entity = JsonConvert.DeserializeObject<TruckParam>(messag);

                this.SaveTruckParam(entity);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Passe o objeto TruckParam");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void GetLobInfoMQtt(string topic, string messag)
        {
            List<LobInfo> entity = new List<LobInfo>();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                var lobInfoFilter = JsonConvert.DeserializeObject<LobInfo>(messag);

                entity = this.GetLobInfo(lobInfoFilter);
            }
            else
            {
                entity = this.GetLobInfo();
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }

        public void SaveLobInfoMQtt(string topic, string messag)
        {
            LobInfo entity = new LobInfo();

            if (!string.IsNullOrWhiteSpace(messag))
            {
                //entity = JsonConvert.DeserializeObject<LobInfo>(messag);

                //var sideMimetype = this.GetMimeType(entity.SideImageTruck);
                //var backMimetype = this.GetMimeType(entity.BackImageTruck);

                //entity.SideImageTruck = $"data:{sideMimetype};base64," + entity.SideImageTruck;
                //entity.BackImageTruck = $"data:{backMimetype};base64," + entity.BackImageTruck;
                //entity.TruckId = 1;

                //this.SaveLobInfo(entity);
            }
            else
            {
                mqttClient.Publish($"response_{topic}", "Passe o objeto LobInfo");
            }

            mqttClient.Publish($"response_{topic}", JsonConvert.SerializeObject(entity));
        }


        public string GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }

        public string GetExtensionFromBase64(string Base64)
        {
            if (Base64 == null || String.IsNullOrWhiteSpace(Base64))
                throw new ArgumentNullException("base64");



            string extension = String.Empty;
            var base64lower = Base64.ToLower().Substring(0, 11);
            foreach (var key in _mappingsMime.Keys)
            {
                if (base64lower.Contains(key.ToLower()))
                {
                    extension = _mappingsMime[key];
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(extension))
                extension = ".png";

            return extension;
        }

        public string GetExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
            {
                throw new ArgumentNullException("mimeType");
            }

            string extension = String.Empty;
            foreach (var key in _mappings.Keys)
            {
                if (_mappings[key] == mimeType)
                {
                    extension = key;
                }
            }
            return extension;
        }

        public void NotifyMqttChangeTruck(int TruckId)
        {
            var infosByTruck = this.GetAllConfig(TruckId);

            mqttClient.Publish($"changeTruck", JsonConvert.SerializeObject(infosByTruck));
        }

        #endregion

        #endregion

        #region BalanceInfo

        public BalanceInfo GetBalanceInfo()
        {
            return this.CoreSqlProvider.GetBalanceInfo();
        }

        public void SaveBalanceInfo(BalanceInfo entity)
        {
            if (entity.Id > 0)
                this.UpdateBalanceInfo(entity);
            else
                this.InsertBalanceInfo(entity);
        }

        public void UpdateBalanceInfo(BalanceInfo entity)
        {
            this.CoreSqlProvider.UpdateBalanceInfo(entity);
        }

        public void InsertBalanceInfo(BalanceInfo entity)
        {
            this.CoreSqlProvider.InsertBalanceInfo(entity);
        }

        #endregion

        #region Truck

        public List<Truck> GetTrucks()
        {
            var trucks = this.CoreSqlProvider.GetTrucks();

            if(trucks != null && trucks.Count > 0)
            {
                foreach(var item in trucks)
                {
                    item.TruckParam = this.GetTruckParam(item.Id);
                }
            }

            return trucks;
        }
        public List<Truck> GetTrucks(Truck entity)
        {
            var trucks = this.CoreSqlProvider.GetTrucks(entity);

            if (trucks != null && trucks.Count > 0)
            {
                foreach (var item in trucks)
                {
                    item.TruckParam = this.GetTruckParam(item.Id);
                }
            }

            return trucks;
        }

        public int SaveTruck(Truck entity)
        {
            if(entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateTruck(entity);
            }
            else
            {
                var existSameTruckByName = this.GetTrucks(new Truck()
                {
                    Name = entity.Name
                }).FirstOrDefault();

                if(existSameTruckByName != null && existSameTruckByName.Id > 0)
                {
                    entity.Id = existSameTruckByName.Id;

                    this.CoreSqlProvider.UpdateTruck(entity);
                }
                else
                    this.CoreSqlProvider.InsertTruck(entity);
            }

            if (entity.TruckParam != null)
            {
                entity.TruckParam.TruckId = entity.Id;

                this.SaveTruckParam(entity.TruckParam);
            }

            return entity.Id;
        }
        public TruckParam GetTruckParam(int TruckId)
        {
            return this.CoreSqlProvider.GetTruckParam(TruckId);
        }

        public int SaveTruckParam(TruckParam entity)
        {
            if (entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateTruckParam(entity);
            }
            else
            {
                if(entity.TruckId > 0)
                {
                    var existTruckParam = this.GetTruckParam(entity.TruckId.Value);

                    if (existTruckParam != null)
                    {
                        entity.Id = existTruckParam.Id;

                        this.CoreSqlProvider.UpdateTruckParam(entity);
                    }
                    else
                    {
                        this.CoreSqlProvider.InsertTruckParam(entity);
                    }
                }
                else
                {
                    this.CoreSqlProvider.InsertTruckParam(entity);
                }

            }

            return entity.Id;
        }

        #endregion

        #region LobInfo

        public List<LobInfo> GetLobInfo()
        {
            var list = this.CoreSqlProvider.GetLobInfo();
            var back = this.CoreSqlProvider.GetLobInfo1();
            var front = this.CoreSqlProvider.GetLobInfo2();

            var newList = new List<LobInfo>();

            //var obj = new LobInfo();


            if (!string.IsNullOrWhiteSpace(back.BackImageTruck) && !string.IsNullOrWhiteSpace(front.SideImageTruck))
            {
                var returnObj = new LobInfo();
                returnObj.Truck = this.GetTrucks(new Truck() { Id = 1 }).FirstOrDefault(); ;
                returnObj.SideImageTruck = front.SideImageTruck;
                returnObj.BackImageTruck = back.BackImageTruck;

                newList.Add(returnObj);
            }

            //foreach (var item in list)
            //{
            //    if (!string.IsNullOrWhiteSpace(obj.BackImageTruck) && !string.IsNullOrWhiteSpace(obj.SideImageTruck))
            //        obj = new LobInfo();

            //    if (!string.IsNullOrWhiteSpace(item.BackImageTruck))
            //        obj.BackImageTruck = item.BackImageTruck;

            //    if (!string.IsNullOrWhiteSpace(item.SideImageTruck))
            //        obj.SideImageTruck = item.SideImageTruck;

            //    if (item.TruckId > 0)
            //        item.Truck = this.GetTrucks(new Truck() { Id = item.TruckId.Value }).FirstOrDefault();


            //    if (!string.IsNullOrWhiteSpace(obj.BackImageTruck) && !string.IsNullOrWhiteSpace(obj.SideImageTruck))
            //    {
            //        var returnObj = new LobInfo();
            //        returnObj = item;
            //        returnObj.SideImageTruck = obj.SideImageTruck;
            //        returnObj.BackImageTruck = obj.BackImageTruck;

            //        newList.Add(returnObj);
            //    }
            //}

            newList = newList.OrderByDescending(x => x.CreateDate).ToList();

            return newList;
        }
        public List<LobInfo> GetLobInfo(LobInfo entity)
        {
            var list = this.CoreSqlProvider.GetLobInfo(entity);

            var obj = new LobInfo();

            var newList = new  List<LobInfo>();

            foreach (var item in list)
            {

                if (!string.IsNullOrWhiteSpace(obj.BackImageTruck) && !string.IsNullOrWhiteSpace(obj.SideImageTruck))
                    obj = new LobInfo();

                if (!string.IsNullOrWhiteSpace(item.BackImageTruck))
                    obj.BackImageTruck = item.BackImageTruck;

                if (!string.IsNullOrWhiteSpace(item.SideImageTruck))
                    obj.SideImageTruck = item.SideImageTruck;

                if (item.TruckId > 0)
                    item.Truck = this.GetTrucks(new Truck() { Id = item.TruckId.Value }).FirstOrDefault();


                if (!string.IsNullOrWhiteSpace(obj.BackImageTruck) && !string.IsNullOrWhiteSpace(obj.SideImageTruck))
                {
                    var returnObj = new LobInfo();
                    returnObj = item;
                    returnObj.SideImageTruck = obj.SideImageTruck;
                    returnObj.BackImageTruck = obj.BackImageTruck;

                    newList.Add(returnObj);
                }
            }



            return newList;
        }

        public int SaveLobInfo(LobInfo entity)
        {
            if (entity.Id > 0)
            {
                this.CoreSqlProvider.UpdateLobInfo(entity);
            }
            else
            {
                this.CoreSqlProvider.InsertLobInfo(entity);
            }

            return entity.Id;
        }

        public dynamic GetAllConfig(int TruckId)
        {
            var truckInfo = this.GetTrucks(new Truck() { Id = TruckId }).FirstOrDefault();
            var balanceInfo = this.GetBalanceInfo();


            return new
            {
                TruckInfo = truckInfo,
                BalanceInfo = balanceInfo
            };
        }

        #endregion
    }
}
