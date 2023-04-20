namespace KisekaeImporter.SubCodes
{
	public class KisekaeLegSize : KisekaeSubCode
	{
		public KisekaeLegSize() : base("cg") { }

		public int LeftLegRotation
		{
			get { return GetInt(0); }
			set { Set(0, value.ToString()); }
		}

		public int LeftLegScaleX
		{
			get { return GetInt(1); }
			set { Set(1, value.ToString()); }
		}

		public int LeftLegScaleY
		{
			get { return GetInt(2); }
			set { Set(2, value.ToString()); }
		}

		public int LeftLegOffsetX
		{
			get { return GetInt(3); }
			set { Set(3, value.ToString()); }
		}

		public int LeftLegOffsetY
		{
			get { return GetInt(4); }
			set { Set(4, value.ToString()); }
		}

		public int RightLegRotation
		{
			get { return GetInt(5); }
			set { Set(5, value.ToString()); }
		}

		public int RightLegScaleX
		{
			get { return GetInt(6); }
			set { Set(5, value.ToString()); }
		}

		public int RightLegScaleY
		{
			get { return GetInt(7); }
			set { Set(7, value.ToString()); }
		}

		public int RightLegOffsetX
		{
			get { return GetInt(8); }
			set { Set(8, value.ToString()); }
		}

		public int RightLegOffsetY
		{
			get { return GetInt(9); }
			set { Set(9, value.ToString()); }
		}

		public bool LeftLegVisible
		{
			get { return GetBool(10); }
			set { Set(10, value); }
		}

		public bool RightLegVisible
		{
			get { return GetBool(11); }
			set { Set(11, value); }
		}
	}
}
