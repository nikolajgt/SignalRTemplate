<template>
  <div class="home">
    <div class="send-message">
      <input v-model="Message" placeholder="Message"/>
      <input v-model="Group" placeholder="Group name" />
      <button @click="sendMessage()" >Send Message</button>
      <button @click="sendMessageCaller()" >Send Message Caller</button>
      <button @click="sendMessageGroup()" >Send Message Group</button>
    </div>
    <div class="groups">
        <input v-model="Group" />
        <button @click="addGroup()" >Add to group</button>
        <button @click="leaveGroup()" >Leave group</button>
    </div>
  </div>
</template>

<script>

import * as signalR from "@microsoft/signalr";

const access_token = localStorage.getItem("access_token");

let connection = new signalR.HubConnectionBuilder()
 .withUrl("http://localhost:7073/ServerHub", {
   accessTokenFactory: () => access_token
 })
 .withAutomaticReconnect()
 .build();



connection.start().then(() => {
 console.log('ServerHub Connected')
})

//CHAT BROADCAST
connection.on("broadcastMessage", function (user, message) {
 console.log(user + message)
})

//ADD OG LEAVE GROUP
connection.on("GroupSend", function(groupName) {
  console.log(groupName)
})

export default {
  name: 'Home',
  data() {
    return {
      Message: '',
      Username: 'Daas',
      UserChat: [],

      Group: '',
    }
  },
  methods: {
    sendMessage() {
       connection.invoke("SendMessage", this.Username, this.Message).catch(err => console.log(err));
    },
    sendMessageCaller() {
      connection.invoke("SendMessageToCaller", this.Username, this.Message)
    },
    sendMessageGroup() {
      connection.invoke("SendMessageToGroup", this.Username, this.Message, this.Group)
    },
    addGroup() {
      connection.invoke("AddToGroup", this.Group)
    },
    leaveGroup() {
      connection.invoke("RemoveFromGroup", this.Group)
    }
  },
  
  components: {
    
  }
}
</script>

<style lang="scss">
.home {
  width: 800px;
  height: 800px;
  display:flex;
  flex-direction: column;
  .send-message {
      padding: 20px;
  }
};

</style>
