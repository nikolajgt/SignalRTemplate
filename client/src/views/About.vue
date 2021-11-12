<template>
  <div id="about">
    <h1>ASP.NET Core SignalR Stock Ticker Sample</h1>

    <input type="button" @click="startStream()" value="Open Market" />
    <input type="button" id="close" value="Close Market" disabled="disabled" />
    <input type="button" id="reset" value="Reset" />

    <h2>Live Stock Table</h2>
    <div id="stockTable">
        <table border="1" id="table">
            <thead>
                <tr><th>Symbol</th><th>Price</th><th>Open</th><th>High</th><th>Low</th><th>Change</th><th>%</th></tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>

    <h2>Live Stock Ticker</h2>
    <div id="stockTicker">
        <div class="inner">
            <ul position="absolute" id="ul">
            </ul>
        </div>
    </div>
  </div>
</template>

<script >
import * as signalR from "@microsoft/signalr";

    const access_token = localStorage.getItem("access_token");

      let connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:7073/StockTickerHub", {
        accessTokenFactory: () => access_token
      })
      .withAutomaticReconnect()
      .build();


 

export default {
  name: 'about',
  data() {
    return {
      stockTable: null,
      stockTableBody: null,
      stockTicker: null,
      stockTickerBody: null,

      rowTemplate:'<td>{symbol}</td><td>{price}</td><td>{dayOpen}</td><td>{dayHigh}</td><td>{dayLow}</td><td class="changeValue"><span class="dir {directionClass}">{direction}</span> {change}</td><td>{percentChange}</td>',
      tickerTemplate: '<span class="symbol">{symbol}</span> <span class="price">{price}</span> <span class="changeValue"><span class="dir {directionClass}">{direction}</span> {change} ({percentChange})</span>',
      up: '▲',
      down: '▼',

      pos: 30,
      tickerInterval: Number,
    }
  },
  methods: {
    startStream: function() {
      connection.invoke("OpenMarket");
    },

    stopStream: function() {
      connection.invoke("CloseMarket");
    },

    resetStream: function() {
      connection.invoke("Reset").then(function () {
            connection.invoke("GetAllStocks").then(function (stocks) {
                for (let i = 0; i < stocks.length; ++i) {
                    this.displayStock(stocks[i]);
                }
            });
        });
    },


    movetIcker: function() {
      this.pos--;
    if (this.pos < -600) {
        this.pos = 500;
    }

    this.stockTickerBody.style.marginLeft = this.pos + 'px';
    },

    marketOpened: function() {
      this.tickerInterval = setInterval(this.moveTicker, 20);
      document.getElementById('open').setAttribute("disabled", "disabled");
      document.getElementById('close').removeAttribute("disabled");
      document.getElementById('reset').setAttribute("disabled", "disabled");
    },

    marketClosed: function() {
      if (this.tickerInterval) {
        clearInterval(this.tickerInterval);
      }
        document.getElementById('open').removeAttribute("disabled");
    document.getElementById('close').setAttribute("disabled", "disabled");
    document.getElementById('reset').removeAttribute("disabled");
    },

    displayStock: function(stock) {
      var displayStock = this.formatStock(stock);
      this.addOrReplaceStock(this.stockTableBody, displayStock, 'tr', this.rowTemplate);
      this.addOrReplaceStock(this.stockTickerBody, displayStock, 'li', this.tickerTemplate);
    },

    addOrReplaceStock: function(table, stock, type, template)  {
      var child = this.createStockNode(stock, type, template);

    // try to replace
    var stockNode = document.querySelector(type + "[data-symbol=" + stock.symbol + "]");
    if (stockNode) {
        var change = stockNode.querySelector(".changeValue");
        var prevChange = parseFloat(change.childNodes[1].data);
        if (prevChange > stock.change) {
            child.className = "decrease";
        }
        else if (prevChange < stock.change) {
            child.className = "increase";
        }
        else {
            return;
        }
        table.replaceChild(child, stockNode);
    } else {
        // add new stock
        table.appendChild(child);
    }
    },

    formatStocks: function(stock) {
      stock.price = stock.price.toFixed(2);
      stock.percentChange = (stock.percentChange * 100).toFixed(2) + '%';
      stock.direction = stock.change === 0 ? '' : stock.change >= 0 ? this.up : this.down;
      stock.directionClass = stock.change === 0 ? 'even' : stock.change >= 0 ? 'up' : 'down';
      return stock;
    },

    createStockNode: function(stock, type, template) {
      var child = document.createElement(type);
      child.setAttribute('data-symbol', stock.symbol);
      child.setAttribute('class', stock.symbol);
      child.innerHTML = template.supplant(stock);
      return child;
    }
  },
  mounted() {
    this.stockTable = document.getElementById('stockTable');
    this.stockTableBody = this.stockTable.getElementsByTagName('tbody')[0];
    this.stockTicker = document.getElementById('stockTicker');
    this.stockTickerBody = this.stockTicker.getElementsByTagName('ul')[0];

  },
  updated() {
    signalR();
  }
}



</script>


<style lang="scss" scoped>

</style>