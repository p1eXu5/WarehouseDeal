<Query Kind="Program" />


void Main()
{
	string s1 = "01.02.2018 8:08";
	string s2 = "28.02.2018 18:21";
	string[] ss = new string[] {s1, s2 };
	
	DateTime dt1;
	
	foreach (System.Globalization.CultureInfo cult 
		in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)) {
	
		if (cult.Name.StartsWith("ru")) Console.WriteLine(cult.Name);
	}
	
	System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ru-RU");
	
	foreach (string str in ss) {
		
		bool res = DateTime.TryParseExact(str, "g", culture, System.Globalization.DateTimeStyles.None, out dt1);
		if (res) Console.WriteLine(dt1);
	}
}

// Define other methods and classes here
