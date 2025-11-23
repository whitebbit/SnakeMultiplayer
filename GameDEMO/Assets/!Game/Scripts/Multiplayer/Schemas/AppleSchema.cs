// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class AppleSchema : Schema {
	[Type(0, "ref", typeof(Vector2Schema))]
	public Vector2Schema position = new Vector2Schema();

	[Type(1, "uint32")]
	public uint id = default(uint);
}

