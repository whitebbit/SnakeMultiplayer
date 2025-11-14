import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

type Vector3 = { x: number; y: number; z: number };
type Vector2 = { x: number; y: number };

export class Vector3Schema extends Schema {
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    set(vector: Vector3) {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }
}

export class Vector2Schema extends Schema {
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    set(vector: Vector2) {
        this.x = vector.x;
        this.y = vector.y;
    }
}

export class Player extends Schema {
    
    @type(Vector3Schema)
    position = new Vector3Schema();

    @type(Vector3Schema)
    velocity = new Vector3Schema();

    @type(Vector2Schema)
    rotation = new Vector2Schema();

    @type("number")
    speed = 0;

    @type("int8")
    maxHP = 0;

    @type("int8")
    currentHP = 0;

    @type("uint8")
    loss = 0;

    @type("int8")
    wI = 0;

    @type("int8")
    sI = 0;

    setPosition(vector: Vector3) {
      const position = new Vector3Schema();

      position.x = vector.x;
      position.y = vector.y;
      position.z = vector.z;

      this.position = position;
    }

    setRotation(vector: Vector2) {
      const rotation = new Vector2Schema();

      rotation.x = vector.x;
      rotation.y = vector.y;

      this.rotation = rotation;
    }

    setVelocity(vector: Vector3) {
      const velocity = new Vector3Schema();

      velocity.x = vector.x;
      velocity.y = vector.y;
      velocity.z = vector.z;

      this.velocity = velocity;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any, skin: any) {
        const player = new Player();

        player.sI = skin;
        player.maxHP = data.hp;
        player.currentHP = data.hp;
        player.speed = data.speed;

        if (data.pos)
          player.setPosition(data.pos);
        
        if (data.rot)
          player.setRotation(data.rot);
        
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {
        const player = this.players.get(sessionId);
        
        if (data.pos)
          player.setPosition(data.pos);
        
        if (data.rot)
          player.setRotation(data.rot);
        
        if (data.vel)
          player.setVelocity(data.vel);
    }
}

export class StateHandlerRoom extends Room<State> {

  maxClients = 2;
  spawnPointsCount = 0;
  skins: number[] = [];

  playerSpawnIndexes: Map<string, number> = new Map();

  mixArray(arr){
    var currentIndex = arr.length;
    var tempValue, randomIndex;

    while(currentIndex !== 0){
      randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex -= 1;
      tempValue = arr[currentIndex];
      arr[currentIndex] = arr[randomIndex];
      arr[randomIndex] = tempValue;
    }

  }

  onCreate(options: any) {

      for (let index = 0; index < options.skins; index++) {
        this.skins.push(index)
      }

      this.mixArray(this.skins);

      this.spawnPointsCount = options.spawnsCount;
      this.setState(new State());

      this.onMessage("move", (client, data) => {
          this.state.movePlayer(client.sessionId, data);
      });

      this.onMessage("shoot", (client, data) => {
          this.broadcast("Shoot", data, { except: client });
      });

      this.onMessage("damage", (client, data) => {
          const targetId = data.id;
          const player = this.state.players.get(targetId);
          if (!player) return;

          let hp = player.currentHP - data.value;

          if (hp > 0) {
              player.currentHP = hp;
              return;
          }

          player.loss += 1;
          player.currentHP = player.maxHP;

          const newIndex = this.getFreeSpawnPoint();
          this.playerSpawnIndexes.set(targetId, newIndex);

          const clnt = this.clients.find(c => c.sessionId === targetId);
          if (clnt)
            clnt.send("Respawn", newIndex);
      });

      this.onMessage("weapon_switch", (client, data) => {
          const player = this.state.players.get(client.sessionId);
          player.wI = data.wI;
      });
  }

  onAuth(client, options, req) {
      return true;
  }

  onJoin(client: Client, data: any) {
      if (this.clients.length > 1) this.lock();

      const skin = this.skins[this.clients.length - 1];

      this.playerSpawnIndexes.set(client.sessionId, data.spawn);
      this.state.createPlayer(client.sessionId, data, skin);
  }

  onLeave(client: Client) {
      this.playerSpawnIndexes.delete(client.sessionId);
      this.state.removePlayer(client.sessionId);
  }

  onDispose() {
      console.log("Dispose StateHandlerRoom");
  }

  getFreeSpawnPoint(): number {
    const taken = new Set(this.playerSpawnIndexes.values());
    const allPoints = Array.from({ length: this.spawnPointsCount }, (_, i) => i);
    const freePoints = allPoints.filter(p => !taken.has(p));
    const available = freePoints.length > 0 ? freePoints : allPoints;
    const randomIndex = Math.floor(Math.random() * available.length);

    return available[randomIndex];
  }
}
