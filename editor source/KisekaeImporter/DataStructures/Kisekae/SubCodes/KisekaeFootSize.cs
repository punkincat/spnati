namespace KisekaeImporter.SubCodes
{
	public class KisekaeFootSize : KisekaeSubCode
	{
		public KisekaeFootSize() : base("ch") { }

		public int LeftFootRotation
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftFootScaleX
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftFootScaleY
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftFootOffsetX
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int LeftFootOffsetY
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightFootRotation
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightFootScaleX
		{
			get { return GetInt(6); }
			set { Set(5, value.ToString()); }
		}

		public int RightFootScaleY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int RightFootOffsetX
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int RightFootOffsetY
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public bool LeftFootVisible
		{
			get { return GetBool(10); }
			set { Set(10, value); }
		}

		public bool RightFootVisible
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}
	}
}
