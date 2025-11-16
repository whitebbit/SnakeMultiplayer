// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Player : Schema {
	[Type(0, "ref", typeof(Vector3Schema))]
	public Vector3Schema position = new Vector3Schema();

	[Type(1, "uint8")]
	public byte d = default(byte);

	[Type(2, "int8")]
	public sbyte sI = default(sbyte);
}

