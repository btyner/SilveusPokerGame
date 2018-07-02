import React, { Component } from 'react';
import './App.css';
import PlayerNames from './PlayerNames';

class App extends Component {
  render() {
    return (
      <div className="App">
        <h1 className="uk-heading-primary">Silveus Poker Game</h1>
        <PlayerNames />
      </div>
    );
  }
}

export default App;
