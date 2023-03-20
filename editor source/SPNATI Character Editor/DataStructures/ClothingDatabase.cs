namespace SPNATI_Character_Editor
{
	public static class ClothingDatabase
	{
		public static CountedSet<string> Items { get; set; } = new CountedSet<string>();

		public static void AddClothing(Clothing item)
		{
			if(!string.IsNullOrEmpty(item.Name))
			{
				Items.Add(item.Name);
			}
		}
	}
}
