import React from 'react';
import '../Footer/Footer.css';

function Footer() {

  return (
    <div className="footerStyle">
      <div className="columnStyle">
        <h3>Curabitur</h3>
        <p>Emauris</p>
        <p>Kringilla</p>
        <p>Oin magna sem</p>
        <p>Kelementum</p>
      </div>

      <div className="columnStyle">
        <h3>Fusce</h3>
        <p>Econsectetur</p>
        <p>Ksollitudin</p>
        <p>Omvulputate</p>
        <p>Nunc fringilla tellu</p>
      </div>

      {/* Column 3 */}
      <div className="columnStyle">
        <h3>Kontakt</h3>
        <div className="subColumns"> {/* Added this new container for sub-columns */}
          <div className="subColumnStyle">
            <strong>Peakontor: Tallinnas</strong>
            <p>Väike-Ameerika 1, 11415 Tallinn</p>
            <p>Telefon: 605 4450</p>
            <p>Faks: 605 3186</p>
          </div>
          <div className="subColumnStyle">
            <strong>Harukontor: Võrus</strong>
            <p>Oja tn 7 (külastusaadress)</p>
            <p>Telefon: 605 3330</p>
            <p>Faks: 605 3155</p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Footer;