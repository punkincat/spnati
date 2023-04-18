namespace KisekaeImporter.SubCodes
{
	public class KisekaeThighSize : KisekaeSubCode
	{
		public KisekaeThighSize() : base("cf") { }

		public int LeftThighRotation
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftThighScaleX
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftThighScaleY
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftThighOffsetX
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int LeftThighOffsetY
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightThighRotation
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightThighScaleX
		{
			get { return GetInt(6); }
			set { Set(5, value.ToString()); }
		}

		public int RightThighScaleY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int RightThighOffsetX
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int RightThighOffsetY
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public bool LeftThighVisible
		{
			get { return GetBool(10); }
			set { Set(10, value); }
		}

		public bool RightThighVisible
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}
	}
}
