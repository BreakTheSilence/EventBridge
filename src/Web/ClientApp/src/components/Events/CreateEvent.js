import CreateEventForm from "../Events/CreateEventForm"
import TitleHeader from "../TitleHeader";

function CreateEvent() {

  return (
    <div className="create-event-container">
      <TitleHeader pageTitle="Create Event" />
      {/*<div className="create-event-image-container">*/}
      {/*  <div>*/}
      {/*    <p>Add new event</p>*/}
      {/*  </div>*/}
      {/*  <img src={leaves} alt="libled" style={{height: 60}} className="create-event-image"/>*/}
      {/*</div>*/}
      <CreateEventForm/>
    </div>
  )
}

export default CreateEvent;