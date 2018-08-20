<template>
  <div class="home">
    <h1>RealTime Status Monitor</h1>
    <p>Machine List</p>
    <ul v-for="v in values" :key="v.Id">
      <li>
        <b>{{v.Id}}</b>: <i>{{v.Active}} ({{v.Processes.length}})</i>
        <p v-for="p in v.Processes" :key="p">
          <i>{{p}}</i>
        </p> 
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import * as signalR from '@aspnet/signalr';

export default Vue.extend({
  data: () => ({
    values: [],
    interval: "",
    apiBase: "http://localhost:54926"
  }),
  
  mounted: function() {
    this.autoRefresh();
  },

  beforeMount: async function() {
    try {
      this.autoRefresh();
      this.values = [];

      const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${this.apiBase}/processHub`)
        .build();
      hubConnection.on("OnReceiveStatus", data => {
        this.values = JSON.parse(data);
      });
      hubConnection.start().catch(err => console.error(err.toString()));
    } catch (err) {
      console.log(err);
    }
  },

  methods: {
    autoRefresh: function() {
      this.interval = setInterval(async () => {
        const response = await fetch(`${this.apiBase}/api/processes`);
      }, 5000);
    }
  }
});
</script>

<style scoped>
  li {
    list-style: none;
  }
</style>