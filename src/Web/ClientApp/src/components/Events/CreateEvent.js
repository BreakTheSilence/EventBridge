import CreateEventForm from "../Events/CreateEventForm"
import leaves from "../../resources/libled.jpg"

function CreateEvent() {

  return (
    <div className="create-event-container">
      <div className="create-event-image-container">
        <div>
          <p>Add new event</p>
        </div>
        <img src={leaves} alt="libled" style={{height: 60}} className="create-event-image"/>
      </div>
      <CreateEventForm/>
    </div>
  )
}

export default CreateEvent;