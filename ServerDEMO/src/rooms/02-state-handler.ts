import { Room, Client } from "colyseus";
import { Schema, type, MapSchema, ArraySchema } from "@colyseus/schema";

type Vector3 = { x: number; y: number; z: number };
type Vector2 = { x: number; y: number };

export class Vector3Schema extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 256) - 128;

    @type("number")
    y = 0;

    @type("number")
    z = Math.floor(Math.random() * 256) - 128;
}

export class Vector2Schema extends Schema {
    @type("number")
    x = Math.floor(Math.random() * 256) - 128;

    @type("number")
    y = Math.floor(Math.random() * 256) - 128;;
}

export class Player extends Schema {
    
    @type(Vector3Schema)
    position = new Vector3Schema();

    @type("uint8")
    d = 2;

    @type("int8")
    sI = 0;

    setPosition(vector: Vector3) {
      const position = new Vector3Schema();

      position.x = vector.x;
      position.y = vector.y;
      position.z = vector.z;

      this.position = position;
    }
}

export class AppleSchema extends Schema {
    @type(Vector2Schema)
    position = new Vector2Schema();

    @type("uint32")
    id = 0;

    setPosition(vector: Vector2) {
        const position = new Vector2Schema();
  
        position.x = vector.x;
        position.y = vector.y;
  
        this.position = position;
    }
}

export class State extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([AppleSchema]) apples = new ArraySchema<AppleSchema>();

    appleLastId = 0;

    createApple() {
        const apple = new AppleSchema();
        
        apple.id = this.appleLastId++;

        this.apples.push(apple);
        
    }

    collectApple(player: Player, data: any) {
        const apple = this.apples.find(apple => apple.id === data.id);
        if (apple) {

            const x = Math.floor(Math.random() * 256) - 128;
            const y = Math.floor(Math.random() * 256) - 128;

            apple.setPosition({ x, y });
        }
    }

    createPlayer(sessionId: string, data: any, skin: any) {
        const player = new Player();

        player.sI = skin;

        if (data.pos)
          player.setPosition(data.pos);
        
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {
        const player = this.players.get(sessionId);
        
        if (data.pos)
          player.setPosition(data.pos);
    }
}

export class StateHandlerRoom extends Room<State> {

  maxClients = 2;
  startAplleCount = 100;
  skins: number[] = [];

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

      for (let index = 0; index < options.sC; index++) {
        this.skins.push(index)
      }

      this.mixArray(this.skins);

      this.setState(new State());

      this.onMessage("move", (client, data) => {
          this.state.movePlayer(client.sessionId, data);
      });

      this.onMessage("collect", (client, data) => {    
        const player = this.state.players.get(client.sessionId);
        this.state.collectApple(player, data);
      });

      for (let index = 0; index < this.startAplleCount; index++) {
        this.state.createApple();
      }
  }

  onAuth(client, options, req) {
      return true;
  }

  onJoin(client: Client, data: any) {
      if (this.clients.length > 1) this.lock();

      const skin = this.skins[this.clients.length - 1];

      this.state.createPlayer(client.sessionId, data, skin);
  }

  onLeave(client: Client) {
      this.state.removePlayer(client.sessionId);
  }

  onDispose() {
      console.log("Dispose StateHandlerRoom");
  }
}
