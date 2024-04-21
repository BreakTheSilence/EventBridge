import React, {useState} from 'react';
import {useNavigate} from "react-router-dom";
import {ModifyParticipantCommand, ParticipantsClient} from "../../web-api-client.ts";
import TextField from "@mui/material/TextField";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";
import {IdCodeValidationService} from "../../services/IdCodeValidationService";
import Alert from '@mui/material/Alert';
import CheckIcon from '@mui/icons-material/Check';

function EditParticipantDetails({participantDto: ParticipantDto}) {
  const [isModified, modifyIsModified] = useState(false);
  const [participant, modifyParticipant] = useState(ParticipantDto);
  const navigate = useNavigate();
  const isCompany = participant.type === 1;
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);

  const handleParticipantChange = (e) => {
    const {name, value} = e.target;
    modifyParticipant(prevParticipant => ({
      ...prevParticipant,
      [name]: value
    }));
    modifyIsModified(true);
  };


  function handeBackClick() {
    navigate(-1);
  }

  async function handeSaveParticipant() {
    if (!isModified) return;
    const codeValidationService = new IdCodeValidationService();

    if (!codeValidationService.isIdCodeValid(participant.idCode)) {
      alert("Invalid ID code. Please enter a valid 11-digit ID code.");
      return;
    }
    let client = new ParticipantsClient();

    let command: ModifyParticipantCommand = {
      id: participant.id,
      firstName: isCompany ? undefined : participant.firstName,
      lastName: isCompany ? undefined : participant.lastName,
      name: isCompany ? participant.name : undefined,
      idCode: participant.idCode
    };
    try {
      await client.updateParticipant(participant.id, command)
    } catch (error) {
      alert("Failed to save data. Please, try again later");
      return;
    }
    setShowSuccessAlert(true);
    setTimeout(() => setShowSuccessAlert(false), 5000);
    modifyIsModified(false);
  }

  return (
    <div>
      {showSuccessAlert && (
        <Alert icon={<CheckIcon fontSize="inherit"/>} severity="success">
          Data is updated successfully.
        </Alert>
      )}
      <h2 style={{marginBottom: 20}}>Participant Details</h2>
      {participant.type === 0 ? (
        <>
          <TextField
            label="First Name"
            name="firstName"
            value={participant.firstName || ''}
            onChange={handleParticipantChange}
            margin="normal"
            fullWidth
            required
          />
          <TextField
            label="Last Name"
            name="lastName"
            value={participant.lastName || ''}
            onChange={handleParticipantChange}
            margin="normal"
            fullWidth
            required
          />
        </>
      ) : (
        <>
          <TextField
            label="Name"
            name="name"
            value={participant.name || ''}
            onChange={handleParticipantChange}
            margin="normal"
            fullWidth
            required
          />
        </>
      )}
      <TextField
        label={isCompany ? "Reg. code" : "ID code"}
        type="number"
        name="idCode"
        value={participant.idCode || ''}
        onChange={handleParticipantChange}
        margin="normal"
        fullWidth
        required
      />
      <div>
        <Stack spacing={2} direction="row" style={{marginTop: 30}}>
          <Button onClick={handeBackClick} variant="outlined">Back</Button>
          <Button onClick={handeSaveParticipant} variant="contained">Save</Button>
        </Stack>
      </div>
    </div>
  );
}

export default EditParticipantDetails