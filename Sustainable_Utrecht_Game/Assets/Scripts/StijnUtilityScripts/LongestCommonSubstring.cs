using System;
using System.Linq;

public class StringStuff{
	public static string findstem(String[] arr, bool CaseSensitive = false) {
        int n = arr.Length;

        String s = arr[0].ToLower();
        int len = s.Length;

        String res = "";

        for(int i = 0; i < len; i++) {
            for(int j = i + 1; j <= len; j++) {

                String stem = s.Substring(i, j - i);
                int k = 1;
                for(k = 1; k < n; k++) {
                    if(!arr[k].ToLower().Contains(stem))
                        break;
                }
                if(k == n && res.Length < stem.Length)
                    res = stem;
            }
        }

        return res;
    }

    public static string[] group(String[] arr, bool CaseSensitive = false) {
        return null;
    }
}