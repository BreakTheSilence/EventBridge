import leaves from "../resources/libled.jpg"

function TitleHeader({ pageTitle }) {

  return (
    <div className="create-event-header-container">
      <div className="create-event-title-container">
        <p>{pageTitle}</p>
      </div>
      <img src={leaves} alt="libled" style={{height: 60}} className="create-event-image"/>
    </div>
  );
}

export default TitleHeader;