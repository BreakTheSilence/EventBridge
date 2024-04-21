import React, { useState } from 'react';
import {useNavigate} from "react-router-dom";
import {
  EventParticipantsClient,
  ModifyEventParticipantCommand
} from "../../web-api-client.ts";
import TextField from "@mui/material/TextField";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";
import Alert from '@mui/material/Alert';
import CheckIcon from '@mui/icons-material/Check';

function EditParticipationDetails({eventParticipantDto: EventParticipantDto}) {
  const [isModified, modifyIsModified] = useState(false);
  const [eventParticipant, modifyEventParticipant] = useState(EventParticipantDto)
  const navigate = useNavigate();
  const isCompany = eventParticipant.participant.type === 1;
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    let finalValue = value;
    if (name === "paymentMethod") {
      finalValue = (value === "cash") ? 0 : 1;
    }
    modifyEventParticipant(prevParticipant => ({
      ...prevParticipant,
      [name]: finalValue
    }));
    modifyIsModified(true);
  };


  function handeBackClick() {
    navigate(-1);
  }

  async function handeSaveEventParticipation() {
    if (!isModified) return;
    
    let client = new EventParticipantsClient();
    console.log(eventParticipant.paymentMethod);
    let command: ModifyEventParticipantCommand = {
      eventId: eventParticipant.eventId,
      participantId: eventParticipant.participant.id,
      paymentMethod: eventParticipant.paymentMethod,
      participantsCount: isCompany ? eventParticipant.participantsCount : 1,
      additionalInfo : eventParticipant.additionalInfo
    };
    try{
      await client.updateEventParticipant(eventParticipant.eventId, eventParticipant.participant.id, command);
    } catch (error) {
      alert("Failed to save data. Please, try again later");
      return;
    }
    setShowSuccessAlert(true);
    setTimeout(() => setShowSuccessAlert(false), 5000);
    modifyIsModified(false);
  }

  return(
    <div>
      {showSuccessAlert && (
        <Alert icon={<CheckIcon fontSize="inherit"/>} severity="success">
          Data is updated successfully.
        </Alert>
      )}
      <h2>Participation details</h2>
      <label>
        Payment method:
        <select
          name="paymentMethod"
          value={eventParticipant.paymentMethod === 0 ? "cash" : "bank transfer"}
          onChange={handleInputChange}
        >
          <option value="cash">Cash</option>
          <option value="bank transfer">Bank transfer</option>
        </select>
      </label>
      {eventParticipant.participant.type !== 0 && (
        <TextField
          label="Participants Count"
          type="number"
          name="participantsCount"
          value={eventParticipant.participantsCount || ''}
          onChange={handleInputChange}
          margin="normal"
          fullWidth
        />
      )}
      <TextField
        label="Additional Information"
        multiline
        name="additionalInfo"
        value={eventParticipant.additionalInfo || ''}
        onChange={handleInputChange}
        margin="normal"
        fullWidth
      />
      <div>
        <Stack spacing={2} direction="row">
          <Button onClick={handeBackClick} variant="text">Back</Button>
          <Button onClick={handeSaveEventParticipation} variant="contained">Save</Button>
        </Stack>
      </div>
    </div>
  );
}

export default EditParticipationDetails