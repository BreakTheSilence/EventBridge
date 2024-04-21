import React, {useState} from 'react';
import {EventParticipantsClient, CreateEventNewParticipantCommand, AddEventCommand} from '../../web-api-client.ts';
import {useNavigate} from "react-router-dom";
import {IdCodeValidationService} from "../../services/IdCodeValidationService";

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

    if (!isCompany){
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

  return (
    <div>
      <h2>Add new participant</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            <input
              type="checkbox"
              checked={isCompany}
              onChange={() => setIsCompany(!isCompany)}
            />
            Компания
          </label>
        </div>

        {isCompany ? (
          <div>
            <label>
              Название компании:
              <input
                type="text"
                name="companyName"
                value={formData.companyName}
                onChange={handleChange}
                required
              />
            </label>
            <label>
              Reg. code:
              <input
                type="text"
                name="idCode"
                value={formData.idCode}
                onChange={handleChange}
                required
              />
            </label>
            <label>
              Количество участников:
              <input
                type="number"
                name="participantsCount"
                value={formData.participantsCount}
                onChange={handleChange}
                required
              />
            </label>
          </div>
        ) : (
          <div>
            <label>
              Имя:
              <input
                type="text"
                name="firstName"
                value={formData.firstName}
                onChange={handleChange}
                required
              />
            </label>
            <label>
              Фамилия:
              <input
                type="text"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                required
              />
            </label>
            <label>
              Ид код:
              <input
                type="text"
                name="idCode"
                value={formData.idCode}
                onChange={handleChange}
                required
              />
            </label>
          </div>
        )}

        <label>
          Метод оплаты:
          <select
            name="paymentMethod"
            value={formData.paymentMethod === 0 ? "cash" : "bank transfer"}
            onChange={handleChange}
          >
            <option value="cash">Наличные</option>
            <option value="bank transfer">Банковский перевод</option>
          </select>
        </label>

        <label>
          Дополнительная информация:
          <textarea
            name="additionalInfo"
            value={formData.additionalInfo}
            onChange={handleChange}
            maxLength={isCompany ? 5000 : 1500}
          />
        </label>

        <button type="submit">Отправить</button>
      </form>
    </div>
  );
}

export default NewParticipantForm;
