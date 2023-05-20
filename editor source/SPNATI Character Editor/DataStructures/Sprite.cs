namespace SPNATI_Character_Editor
{
	/// <summary>
	/// In-game (non-epilogue) sprite
	/// </summary>
	public class Sprite : Directive
	{
		public Sprite() { }

		public Sprite(ISkin skin) : base(skin) { }

		public override string ToString()
		{
			return $"Sprite: {Id}";
		}
	}
}