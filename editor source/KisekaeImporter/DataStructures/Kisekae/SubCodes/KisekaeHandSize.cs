namespace KisekaeImporter.SubCodes
{
	public class KisekaeHandSize : KisekaeSubCode
	{
		public KisekaeHandSize() : base("cd") { }

		public int LeftHandScaleX
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftHandScaleY
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftHandOffsetX
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftHandOffsetY
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int RightHandScaleX
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightHandScaleY
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightHandOffsetX
		{
			get { return GetInt(6); }
			set { Set(6, value.ToString()); }
		}

		public int RightHandOffsetY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}
	}
}
