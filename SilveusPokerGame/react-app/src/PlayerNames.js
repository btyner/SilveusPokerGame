import React, { Component } from 'react';
import './PlayerNames.css';
import PokerTable from './PokerTable';

class PlayerNames extends Component {
    constructor() {
        super();
        this.state = {
            txtP1Name: '',
            txtP2Name: '',
            player1Cards: '',
            player2Cards: '',
            playerDataReceived: false,
            winner: ''
        }

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        this.setState({[name]: value});
    }

    handleSubmit(event) {
        event.preventDefault();

        if (this.state.txtP1Name !== '' && this.state.txtP2Name !== '')
        {
            var url = 'http://localhost:9476/api/poker/';
            var data = {Player1: this.state.txtP1Name, Player2: this.state.txtP2Name};

            fetch(url, {
              method: 'POST', // or 'PUT'
              body: JSON.stringify(data), // data can be `string` or {object}!
              headers:{
                'Content-Type': 'application/json'
              }
            }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then((response) => {
                console.log('Success:', response);
                this.setState({ player1Cards: response.player1Hand.cards });
                this.setState({ player2Cards: response.player2Hand.cards });
                this.setState({ playerDataReceived: true });
            });
        }
    }

  render() {
    return (
        <div>
        <div className="player-name-form">
            <form className="uk-form-stacked" onSubmit={this.handleSubmit}>
                <div className="uk-padding-small">
                    <label className="uk-form-label" htmlFor="txtP1Name">Player 1 Name</label>
                    <div className="uk-form-controls">
                        <input type="text" name="txtP1Name" className="uk-input" placeholder="Enter name" value={this.state.txtP1Name} onChange={this.handleChange} />
                    </div>
                </div>
                <div className="uk-padding-small">
                    <div className="uk-form-label" htmlFor="txtP2Name">Player 2 Name</div>
                    <div className="uk-form-controls">
                        <input type="text" name="txtP2Name" className="uk-input" placeholder="Enter name" value={this.state.txtP2Name} onChange={this.handleChange} />
                    </div>
                </div>
                <div className="uk-padding-small">
                    <button className="uk-button uk-button-primary">Start Game</button>
                </div>
            </form>
            </div>
        {
          this.state.playerDataReceived ?
              <PokerTable Player1Data={{name: this.state.txtP1Name, cards: this.state.player1Cards}} Player2Data={{name: this.state.txtP2Name, cards: this.state.player2Cards}} Winner={ this.state.winner } />
          :
              <div></div>
        }
        </div>
    );
  }
}

export default PlayerNames;
