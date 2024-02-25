using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FontFaceGenerator.ViewModels;

public class MainViewModel : ViewModelBase
{
	public MainViewModel()
	{
		OpenFontsFolderDialogCommand = ReactiveCommand.Create(OpenFontsFolderDialog);
		ReadFontsInfoCommand = ReactiveCommand.Create(ReadFontsInfo);
	}

	public ReactiveCommand<Unit, Unit> OpenFontsFolderDialogCommand { get; }
	public ReactiveCommand<Unit, Unit> ReadFontsInfoCommand { get; }

	private Uri? fontsFolder { get; set; } = null;

	async void OpenFontsFolderDialog()
	{
		if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			FolderPickerOpenOptions openOptions = new FolderPickerOpenOptions();
			openOptions.AllowMultiple = false;
			openOptions.Title = "Папка со шрифтами";
			IStorageFolder folder = (await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(openOptions))[0];
			if (folder != null)
			{
				FontsFolderPath = folder.Name;
				fontsFolder = folder.Path;
			}
		}
	}

	async void ReadFontsInfo() 
	{
		if (fontsFolder != null) 
		{
			string htmlFilename = "fonts.html";
			string cssFileName = "fontstyle.css";

			StringBuilder cssContent = new StringBuilder(); 
			StringBuilder htmlContent = new StringBuilder();

			htmlContent.Append("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta name=\"robots\" content=\"noindex, noarchive\">\r\n    <meta name=\"format-detection\" content=\"telephone=no\">\r\n    <title>Transfonter demo</title>\r\n    <link href=\"");
			htmlContent.Append(cssFileName);
			htmlContent.Append("stylesheet.css\" rel=\"stylesheet\">\r\n    <style>\r\n        /*\r\n        http://meyerweb.com/eric/tools/css/reset/\r\n        v2.0 | 20110126\r\n        License: none (public domain)\r\n        */\r\n        html, body, div, span, applet, object, iframe,\r\n        h1, h2, h3, h4, h5, h6, p, blockquote, pre,\r\n        a, abbr, acronym, address, big, cite, code,\r\n        del, dfn, em, img, ins, kbd, q, s, samp,\r\n        small, strike, strong, sub, sup, tt, var,\r\n        b, u, i, center,\r\n        dl, dt, dd, ol, ul, li,\r\n        fieldset, form, label, legend,\r\n        table, caption, tbody, tfoot, thead, tr, th, td,\r\n        article, aside, canvas, details, embed,\r\n        figure, figcaption, footer, header, hgroup,\r\n        menu, nav, output, ruby, section, summary,\r\n        time, mark, audio, video {\r\n            margin: 0;\r\n            padding: 0;\r\n            border: 0;\r\n            font-size: 100%;\r\n            font: inherit;\r\n            vertical-align: baseline;\r\n        }\r\n        /* HTML5 display-role reset for older browsers */\r\n        article, aside, details, figcaption, figure,\r\n        footer, header, hgroup, menu, nav, section {\r\n            display: block;\r\n        }\r\n        body {\r\n            line-height: 1;\r\n        }\r\n        ol, ul {\r\n            list-style: none;\r\n        }\r\n        blockquote, q {\r\n            quotes: none;\r\n        }\r\n        blockquote:before, blockquote:after,\r\n        q:before, q:after {\r\n            content: '';\r\n            content: none;\r\n        }\r\n        table {\r\n            border-collapse: collapse;\r\n            border-spacing: 0;\r\n        }\r\n        /* demo styles */\r\n        body {\r\n            background: #f0f0f0;\r\n            color: #000;\r\n        }\r\n        .page {\r\n            background: #fff;\r\n            width: 920px;\r\n            margin: 0 auto;\r\n            padding: 20px 20px 0 20px;\r\n            overflow: hidden;\r\n        }\r\n        .font-container {\r\n            overflow-x: auto;\r\n            overflow-y: hidden;\r\n            margin-bottom: 40px;\r\n            line-height: 1.3;\r\n            white-space: nowrap;\r\n            padding-bottom: 5px;\r\n        }\r\n        h1 {\r\n            position: relative;\r\n            background: #444;\r\n            font-size: 32px;\r\n            color: #fff;\r\n            padding: 10px 20px;\r\n            margin: 0 -20px 12px -20px;\r\n        }\r\n        .letters {\r\n            font-size: 25px;\r\n            margin-bottom: 20px;\r\n        }\r\n        .s10:before {\r\n            content: '10px';\r\n        }\r\n        .s11:before {\r\n            content: '11px';\r\n        }\r\n        .s12:before {\r\n            content: '12px';\r\n        }\r\n        .s14:before {\r\n            content: '14px';\r\n        }\r\n        .s18:before {\r\n            content: '18px';\r\n        }\r\n        .s24:before {\r\n            content: '24px';\r\n        }\r\n        .s30:before {\r\n            content: '30px';\r\n        }\r\n        .s36:before {\r\n            content: '36px';\r\n        }\r\n        .s48:before {\r\n            content: '48px';\r\n        }\r\n        .s60:before {\r\n            content: '60px';\r\n        }\r\n        .s72:before {\r\n            content: '72px';\r\n        }\r\n        .s10:before, .s11:before, .s12:before, .s14:before,\r\n        .s18:before, .s24:before, .s30:before, .s36:before,\r\n        .s48:before, .s60:before, .s72:before {\r\n            font-family: Arial, sans-serif;\r\n            font-size: 10px;\r\n            font-weight: normal;\r\n            font-style: normal;\r\n            color: #999;\r\n            padding-right: 6px;\r\n        }\r\n        pre {\r\n            display: block;\r\n            padding: 9px;\r\n            margin: 0 0 12px;\r\n            font-family: Monaco, Menlo, Consolas, \"Courier New\", monospace;\r\n            font-size: 13px;\r\n            line-height: 1.428571429;\r\n            color: #333;\r\n            font-weight: normal;\r\n            font-style: normal;\r\n            background-color: #f5f5f5;\r\n            border: 1px solid #ccc;\r\n            overflow-x: auto;\r\n            border-radius: 4px;\r\n        }\r\n        /* responsive */\r\n        @media (max-width: 959px) {\r\n            .page {\r\n                width: auto;\r\n                margin: 0;\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n<div class=\"page\">");



            StringBuilder stringBuilder = new StringBuilder();
			foreach (string fileEntry in Directory.EnumerateFiles(fontsFolder.AbsolutePath, "*.*", SearchOption.TopDirectoryOnly)) 
			{
				if (fileEntry.EndsWith(".ttf") || fileEntry.EndsWith(".otf")) 
				{
					GlyphTypeface ttf = new GlyphTypeface(new Uri(fileEntry));
					//" @font-face\r\n {\r\n    font-family: 'IBM Plex Sans';\r\n    font-style: normal;\r\n    font-weight: 600;\r\n    src: url('../fonts/IBMPlexSans-Medium.ttf') format('truetype');\r\n}"
					/*stringBuilder.AppendLine(ttf.FamilyNames.FirstOrDefault().Value ?? "");
					stringBuilder.AppendLine(ttf.Style.ToString() ?? "");
					stringBuilder.AppendLine(ttf.Stretch.ToString() ?? "");
					stringBuilder.AppendLine(ttf.Weight.ToString() ?? "");*/

					stringBuilder.AppendLine("@font-face\n{");
					stringBuilder.Append("	font-family:'");
					stringBuilder.Append(ttf.FamilyNames.FirstOrDefault().Value);
					stringBuilder.Append("';\n	font-style:");
					stringBuilder.Append(ttf.Style.ToString().ToLower());
					stringBuilder.Append(";\n	font-weight:");
					stringBuilder.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
					stringBuilder.Append(";\n	src: url('");
					
					stringBuilder.Append(OutputFolder);
					stringBuilder.Append(Path.GetFileName(fileEntry));
					stringBuilder.Append("') format('");
					if (fileEntry.EndsWith(".ttf"))
					{
						stringBuilder.Append("truetype");
					}
					else if(fileEntry.EndsWith(".otf")) 
					{
						stringBuilder.Append("opentype");
					}
					stringBuilder.Append("');\n}\n\n");



                    cssContent.AppendLine("@font-face\n{");
                    cssContent.Append("	font-family:'");
                    cssContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    cssContent.Append("';\n	font-style:");
                    cssContent.Append(ttf.Style.ToString().ToLower());
                    cssContent.Append(";\n	font-weight:");
                    cssContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    cssContent.Append(";\n	src: url('");

                    cssContent.Append(Path.GetFileName(fileEntry));
                    cssContent.Append("') format('");
                    if (fileEntry.EndsWith(".ttf"))
                    {
                        cssContent.Append("truetype");
                    }
                    else if (fileEntry.EndsWith(".otf"))
                    {
                        cssContent.Append("opentype");
                    }
                    cssContent.Append("');\n}\n\n");


					htmlContent.Append("\n\n<div class=\"demo\">\r\n        <h1 style=\"font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
					htmlContent.Append("'; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
					htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
                    htmlContent.Append(";\">");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
					htmlContent.Append("</h1>\r\n        <pre title=\"Usage\">.your-style {\r\n    font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    htmlContent.Append("'; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
					htmlContent.Append(";\r\n}</pre>\r\n        <pre title=\"Preload (optional)\">\r\n&lt;link rel=&quot;preload&quot; href=&quot;subset-Rubik-Medium_1.ttf&quot; as=&quot;font&quot; type=&quot;font/ttf&quot; crossorigin&gt;</pre>\r\n        <div class=\"font-container\" style=\"font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value); 
					htmlContent.Append("'; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
                    htmlContent.Append(";\">\r\n            <p class=\"letters\">\r\n                abcdefghijklmnopqrstuvwxyz<br>\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ<br>\r\n                0123456789.:,;()*!?'@#&lt;&gt;$%&^+-=~\r\n            </p>\r\n            <p class=\"s10\" style=\"font-size: 10px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s11\" style=\"font-size: 11px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s12\" style=\"font-size: 12px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s14\" style=\"font-size: 14px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s18\" style=\"font-size: 18px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s24\" style=\"font-size: 24px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s30\" style=\"font-size: 30px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s36\" style=\"font-size: 36px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s48\" style=\"font-size: 48px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s60\" style=\"font-size: 60px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s72\" style=\"font-size: 72px;\">The quick brown fox jumps over the lazy dog.</p>\r\n        </div>\r\n    </div>\n\n");


                    htmlContent.Append("\n\n<div class=\"demo\">\r\n        <h1 style=\"font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    htmlContent.Append("', sans-serif; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
                    htmlContent.Append(";\">");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    htmlContent.Append("</h1>\r\n        <pre title=\"Usage\">.your-style {\r\n    font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    htmlContent.Append("', sans-serif; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
                    htmlContent.Append(";\r\n}</pre>\r\n        <pre title=\"Preload (optional)\">\r\n&lt;link rel=&quot;preload&quot; href=&quot;subset-Rubik-Medium_1.ttf&quot; as=&quot;font&quot; type=&quot;font/ttf&quot; crossorigin&gt;</pre>\r\n        <div class=\"font-container\" style=\"font-family: '");
                    htmlContent.Append(ttf.FamilyNames.FirstOrDefault().Value);
                    htmlContent.Append("', sans-serif; font-weight: ");
                    htmlContent.Append(ttf.Weight.ToString().ToLower() == "black" ? 900 : ttf.Weight.ToString().ToLower());
                    htmlContent.Append("; font-style: ");
                    htmlContent.Append(ttf.Style.ToString().ToLower());
                    htmlContent.Append(";\">\r\n            <p class=\"letters\">\r\n                abcdefghijklmnopqrstuvwxyz<br>\r\nABCDEFGHIJKLMNOPQRSTUVWXYZ<br>\r\n                0123456789.:,;()*!?'@#&lt;&gt;$%&^+-=~\r\n            </p>\r\n            <p class=\"s10\" style=\"font-size: 10px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s11\" style=\"font-size: 11px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s12\" style=\"font-size: 12px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s14\" style=\"font-size: 14px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s18\" style=\"font-size: 18px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s24\" style=\"font-size: 24px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s30\" style=\"font-size: 30px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s36\" style=\"font-size: 36px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s48\" style=\"font-size: 48px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s60\" style=\"font-size: 60px;\">The quick brown fox jumps over the lazy dog.</p>\r\n            <p class=\"s72\" style=\"font-size: 72px;\">The quick brown fox jumps over the lazy dog.</p>\r\n        </div>\r\n    </div>\n\n");
                }
			}
			OutputText = stringBuilder.ToString();
			htmlContent.Append("</div>\r\n</body>\r\n</html>");


            File.WriteAllText(Path.Combine(fontsFolder.AbsolutePath, cssFileName), cssContent.ToString());
			File.WriteAllText(Path.Combine(fontsFolder.AbsolutePath, htmlFilename), htmlContent.ToString());
		}
	}



	private string? _FontsFolderPath;

	public string? FontsFolderPath
	{
		get
		{
			return _FontsFolderPath;
		}
		set
		{
			this.RaiseAndSetIfChanged(ref _FontsFolderPath, value);
		}
	}

	private string? _OutputText; 

	public string? OutputText
	{
		get
		{
			return _OutputText;
		}
		set
		{
			// We can use "RaiseAndSetIfChanged" to check if the value changed and automatically notify the UI
			this.RaiseAndSetIfChanged(ref _OutputText, value);
		}
	}

	private string? _OutputFolder;

	public string? OutputFolder
	{
		get
		{
			return _OutputFolder;
		}
		set
		{
			// We can use "RaiseAndSetIfChanged" to check if the value changed and automatically notify the UI
			this.RaiseAndSetIfChanged(ref _OutputFolder, value);
		}
	}
}
