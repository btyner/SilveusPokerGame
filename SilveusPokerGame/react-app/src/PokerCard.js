import React, { Component } from 'react';
import './PokerCard.css';

class PokerCard extends Component {
    constructor() {
        super();
    }

    
    render() {
        const cardSuit = {
            0: "spades",
            1: "hearts",
            2: "diamonds",
            3: "clubs"
        };
        const cardRank = {
            2: "2",
            3: "3",
            4: "4",
            5: "5",
            6: "6",
            7: "7",
            8: "8",
            9: "9",
            10: "10",
            11: "jack",
            12: "queen",
            13: "king",
            14: "ace"
        };

        const filename = "/assets/cards/" + cardRank[this.props.rank] + "_of_" + cardSuit[this.props.suit] + ".png";

        return (
            <li className="card-face">
                <img src={filename} />
            </li>
        );
    }
}

export default PokerCard