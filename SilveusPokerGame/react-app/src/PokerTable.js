import React, { Component } from 'react';
import './PokerTable.css';
import PokerCard from './PokerCard';

class PokerTable extends Component {
    constructor() {
        super();

        this.state = {
            winner: '',
            type: ''
        }

        this.handleGetWinner = this.handleGetWinner.bind(this);

    }

    handleGetWinner() {

        fetch('http://localhost:9476/api/poker/getwinner', {
          headers:{
            'Content-Type': 'application/json'
          }
        }).then(res => res.json())
        .catch(error => console.error('Error:', error))
        .then((response) => {
            console.log('Success:', response);
            this.setState({ winner: response.winner });
            this.setState({ type: response.type });
        });
    }

    componentWillReceiveProps(nextProps){
        if(nextProps.Winner !== this.state.winner){
            this.setState({winner: nextProps.Winner});
        }
    }

    render() {

        const handTypes = {
            "RoyalFlush": "a Royal Flush",
            "StraightFlush": "a Straight Flush",
            "FourOfAKind": "a Four-Of-A-Kind",
            "FullHouse": "a Full House",
            "Flush": "a Flush",
            "Straight": "a Straight",
            "ThreeOfAKind": "a Three-Of-A-Kind",
            "TwoPairs": "Two Pairs",
            "OnePair": "a Pair",
            "HighCard": "the highest valued card"
        };

        const p1cards = [];
        const p2cards = [];

        for (var i = 0; i < this.props.Player1Data.cards.length; i += 1) {
          p1cards.push(<PokerCard rank={this.props.Player1Data.cards[i].rank} suit={this.props.Player1Data.cards[i].suit} />);
        };

        for (var i = 0; i < this.props.Player2Data.cards.length; i += 1) {
          p2cards.push(<PokerCard rank={this.props.Player2Data.cards[i].rank} suit={this.props.Player2Data.cards[i].suit} />);
        };

        return (
            <div className="poker-table">
                <div className={(this.state.winner == this.props.Player1Data.name) ? "winner" : "uk-heading-divider"}>
                    <h3 className="uk-heading-divider">{this.props.Player1Data.name} {(this.state.winner == this.props.Player1Data.name) ? " won with " + handTypes[this.state.type] + "!" : ""}</h3>
                    <ul>
                        {p1cards}
                    </ul>
                </div>
                <div className={(this.state.winner == this.props.Player2Data.name) ? "winner" : "uk-heading-divider"}>
                    <h3 className="uk-heading-divider">{this.props.Player2Data.name} {(this.state.winner == this.props.Player2Data.name) ? " won with " + handTypes[this.state.type] + "!" : ""}</h3>
                    <ul>
                        {p2cards}
                    </ul>
                    <div className="uk-padding-small">
                        <button className="uk-button uk-button-primary" onClick={this.handleGetWinner}>Get Winner</button>
                    </div>
                </div>
            </div>
        );
    }
}

export default PokerTable