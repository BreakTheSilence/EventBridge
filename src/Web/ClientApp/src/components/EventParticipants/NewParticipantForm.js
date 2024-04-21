import React, {useState} from 'react';
import {EventParticipantsClient} from '../../web-api-client.ts';
import {useNavigate} from "react-router-dom";
import {IdCodeValidationService} from "../../services/IdCodeValidationService";
import './EventForm.css';
import Button from "@mui/material/Button";

function NewParticipantForm({eventId}) {
  const navigate = useNavigate();
  const [EventId] = useState(eventId)
  const [isCompany, setIsCompany] = useState(false);
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    idCode: '',
    companyName: '',
    participantsCount: '',
    paymentMethod: 0,
    additionalInfo: ''
  });

  const handleChange = (e) => {
    const {name, value} = e.target;
    let finalValue = value;

    if (name === "paymentMethod") {
      finalValue = (value === "cash") ? 0 : 1;
    }

    setFormData(prevState => ({
      ...prevState,
      [name]: finalValue
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const codeValidationService = new IdCodeValidationService();

    if (!isCompany) {
      if (!codeValidationService.isIdCodeValid(formData.idCode)) {
        alert("Invalid ID code. Please enter a valid 11-digit ID code.");
        return; // Stop the form submission if the ID code is invalid
      }
    }

    const newEventParticipant = {
      eventId: eventId,
      participantType: isCompany ? 1 : 0,
      firstName: isCompany ? undefined : formData.firstName,
      lastName: isCompany ? undefined : formData.lastName,
      name: isCompany ? formData.companyName : undefined,
      idCode: formData.idCode,
      participationCount: isCompany ? formData.participantsCount : 1,
      paymentMethod: formData.paymentMethod,
      additionalInfo: formData.additionalInfo
    };

    let client = new EventParticipantsClient();
    await client.createEventParticipantFromNewParticipant(newEventParticipant);
    navigate(0);
  };

  function handleBackButton() {
    navigate(-1);
  }

  return (
    <div className="form-container">
      <h2>Add new participant</h2>
      <form onSubmit={handleSubmit} className="event-form">
        <div className="checkbox-group" style={{marginBottom: 20}}>
          <span>Company</span>
          <input
            type="checkbox"
            checked={isCompany}
            onChange={() => setIsCompany(!isCompany)}
          />
        </div>

        {isCompany ? (
          // If it is a company, show the company fields
          <>
            <div className="form-group">
              <label>Company name:
                <input
                  type="text"
                  name="companyName"
                  value={formData.companyName}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>Reg. code:
                <input
                  type="text"
                  name="idCode"
                  value={formData.idCode}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>Participants count:
                <input
                  type="number"
                  name="participantsCount"
                  value={formData.participantsCount}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
          </>
        ) : (
          // If it is not a company, show the individual fields
          <>
            <div className="form-group">
              <label>First name:
                <input
                  type="text"
                  name="firstName"
                  value={formData.firstName}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>Last name:
                <input
                  type="text"
                  name="lastName"
                  value={formData.lastName}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>ID code:
                <input
                  type="text"
                  name="idCode"
                  value={formData.idCode}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
          </>
        )}

        <div className="form-group">
          <label>Payment method:
            <select
              name="paymentMethod"
              value={formData.paymentMethod === 0 ? "cash" : "bank transfer"}
              onChange={handleChange}
            >
              <option value="cash">Cash</option>
              <option value="bank transfer">Bank transfer</option>
            </select>
          </label>
        </div>

        <div className="form-group">
          <label>Additional information:
            <textarea
              name="additionalInfo"
              value={formData.additionalInfo}
              onChange={handleChange}
              maxLength={isCompany ? 5000 : 1500}
            />
          </label>
        </div>

        <div className="form-actions">
          <Button variant="outlined" onClick={handleBackButton}>Back</Button>
          <Button variant="contained" type="submit">Send</Button>
        </div>
      </form>
    </div>
  );
}

export default NewParticipantForm;
