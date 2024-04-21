import React, {useState} from 'react';
import {AddEventCommand, EventsClient} from '../../web-api-client.ts';
import {useNavigate} from 'react-router-dom';
import './EventForm.css';
import Button from '@mui/material/Button';

function EventForm() {
  const [name, setName] = useState('');
  const [date, setDate] = useState('');
  const [location, setLocation] = useState('');
  const [description, setDescription] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    console.log('Submitted:', {name, date, location, description});

    const newEvent: AddEventCommand = {
      name: name,
      date: date,
      location: location,
      description: description
    };
    let eventsClient = new EventsClient();
    await eventsClient.createEvent(newEvent);
    navigate("/");
  };

  return (
    <div className="form-container">
      <form onSubmit={handleSubmit} className="event-form">
        <div className="form-group">
          <label>Ürituse nimi:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Toimumisaeg:</label>
          <input
            type="text"
            placeholder="pp.kk.aaaa hh:mm"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Koht:</label>
          <input
            type="text"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label>Lisainfo:</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <div className="form-actions">
          <Button variant="outlined" type="button" onClick={() => navigate(-1)}>Tagasi</Button>
          <Button variant="contained" type="submit">Lisa</Button>
        </div>
      </form>
    </div>
  );
}

export default EventForm;