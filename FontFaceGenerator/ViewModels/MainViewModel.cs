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
				}
			}
			OutputText = stringBuilder.ToString();
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
