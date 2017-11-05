﻿using System;

namespace KisekaeImporter
{
	public static class KisekaeSubCodeMap
	{
		/// <summary>
		/// Gets the component type that a subcode ID belongs to
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Type GetComponentType(string id)
		{
			switch (id)
			{
				case "aa":
				case "ab":
				case "ac":
				case "ba":
				case "bc":
				case "bb":
				case "bd":
				case "be":
					return typeof(KisekaePose);
				case "ea":
				case "ec":
				case "ed":
				case "ef":
				case "eg":
				case "r":
					return typeof(KisekaeHair);
				case "ca":
				case "da":
				case "db":
				case "dd":
				case "dh":
				case "di":
				case "qa":
				case "qb":
				case "dc":
				case "eh":
				case "pb":
				case "pc":
				case "pd":
				case "pe":
					return typeof(KisekaeAppearance);
				case "fa":
				case "fb":
				case "fh":
				case "fc":
				case "fj":
				case "fd":
				case "fe":
				case "ff":
				case "fg":
				case "fi":
				case "pa":
				case "t":
					return typeof(KisekaeFace);
				case "ga":
				case "gb":
				case "gc":
				case "gd":
				case "ge":
				case "gh":
				case "gf":
				case "gg":
				case "ha":
				case "hb":
				case "hc":
				case "hd":
					return typeof(KisekaeExpression);
				case "ia":
				case "if":
				case "ib":
				case "id":
				case "ic":
				case "jc":
				case "ie":
				case "ja":
				case "jb":
				case "jd":
				case "je":
				case "jf":
				case "jg":
				case "ka":
				case "kb":
				case "kc":
				case "kd":
				case "ke":
				case "kf":
				case "la":
				case "lb":
				case "lc":
				case "oa":
				case "os":
				case "ob":
				case "oc":
				case "od":
				case "oe":
				case "of":
				case "m":
				case "n":
				case "s":
				case "og":
				case "oh":
				case "oo":
				case "op":
				case "oq":
				case "or":
				case "oi":
				case "oj":
				case "ok":
				case "ol":
				case "om":
				case "on":
					return typeof(KisekaeClothing);
				default:
					return null;
			}
		}
	}
}
