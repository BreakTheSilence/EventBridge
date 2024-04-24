import React, { useState } from 'react';
import { AddEventCommand, EventsClient } from '../../web-api-client.ts';
import { useNavigate } from 'react-router-dom';
import './EventForm.css';
import Button from '@mui/material/Button';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers';
import { TextField } from '@mui/material';
import dayjs from 'dayjs';

function EventForm() {
  const [name, setName] = useState('');
  const [date, setDate] = useState(dayjs());
  const [location, setLocation] = useState('');
  const [description, setDescription] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    console.log('Submitted:', {name, date, location, description});

    const newEvent: AddEventCommand = {
      name: name,
      date: date ? date.format() : null, // Format date or handle null if no date is selected
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
          <label>Event Name:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Event Date:</label>
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DatePicker
              renderInput={(props) => <TextField {...props} />}
              value={date}
              onChange={setDate}
              required
            />
          </LocalizationProvider>
        </div>
        <div className="form-group">
          <label>Location:</label>
          <input
            type="text"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label>Additional Information:</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <div className="form-actions">
          <Button variant="outlined" type="button" onClick={() => navigate(-1)}>Back</Button>
          <Button variant="contained" type="submit">Add Event</Button>
        </div>
      </form>
    </div>
  );
}

export default EventForm;
