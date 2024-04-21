import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Collapse, Navbar, NavbarToggler, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import logo from '../resources/logo.svg'
import './NavMenu.css'; 

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render() {
    return (
      <header className="header-style">
        <Navbar dark expand="md" className="navbar-dark border-bottom box-shadow mb-3">
          <NavbarBrand tag={Link} to="/">
            <img src={logo} alt="Nullam" style={{ height: 50 }} />
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav ml-auto">
              <NavItem>
                <NavLink tag={Link} to="/" className="text-light">Home</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/create-event" className="text-light">Add event</NavLink>
              </NavItem>
              <NavItem>
                <NavLink tag={Link} to="/identity/account/manage" className="text-light">Account</NavLink>
              </NavItem>
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }
}
