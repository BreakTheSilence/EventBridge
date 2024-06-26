import React, {Component} from 'react';
import {Container} from 'reactstrap';
import {NavMenu} from './NavMenu';
import '../custom.css';
import Footer from "./Footer/Footer";

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div className="background-div">
        <div className="site-wrapper">
          <NavMenu/>
          <Container tag="main" className="main-container">
            {this.props.children}
          </Container>
          <Footer/>
        </div>
      </div>
    );
  }
}
