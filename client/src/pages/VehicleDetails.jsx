import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import UserImage from "../components/UserImage";
import { userImages } from "../components/UserImages";

function VehicleDetails() {
  const { name, regNumber } = useParams();
  const decodedName = decodeURIComponent(name);
  const decodedRegNumber = decodeURIComponent(regNumber);

  const [vehicle, setVehicle] = useState(null);
  const image = userImages[decodedName] || userImages.default;

  useEffect(() => {
    const fetchVehicle = async () => {
      try {
        const res = await fetch(
          `http://localhost:5048/api/users/${encodeURIComponent(decodedName)}/vehicles/${encodeURIComponent(decodedRegNumber)}`
        );
        if (!res.ok) throw new Error("Vehicle not found");
        const data = await res.json();
        setVehicle(data);
      } catch (err) {
        console.error(err);
      }
    };

    fetchVehicle();
  }, [decodedName, decodedRegNumber]);

  if (!vehicle) return <p>Loading vehicle...</p>;

  const groupedDates = vehicle.savedDatesCurrentMonth.reduce((acc, vd) => {
    const date = new Date(vd.date);
    const dayKey = date.toISOString().split("T")[0]; // yyyy-mm-dd
    if (!acc[dayKey]) acc[dayKey] = [];
    acc[dayKey].push({ ...vd, date });
    return acc;
  }, {});

  // Sorterade dagar i fallande ordning
  const sortedDays = Object.keys(groupedDates).sort(
    (a, b) => new Date(b) - new Date(a)
  );
  
  return (
    <div className="vehicle-container">
      <h1>Fordonsinfo</h1>
      <div className="profile-details">  
        <div>
          <p>Registreringsnummer: {vehicle.registrationNumber}</p>
        <p>Typ: {vehicle.type}</p>
        <p className="cursive fee">Nuvarande månadsavgift: {vehicle.currentMonthlyFee} SEK</p>
        </div>
        <div className="user-img">
          <UserImage img={image} />
        </div>        
      </div>        
        <div className="dates-list">
        <h2>Taxerade datum denna månad</h2>
        {sortedDays.map((day) => {
          const dayEntries = groupedDates[day];
          const totalFee = dayEntries.reduce((sum, vd) => sum + vd.fee, 0);

          return (
            <div key={day} className="date-group">
              <h3 className="">
                {new Date(day).toLocaleDateString("sv-SE")} {", "}
                <span className="fee">{totalFee} SEK</span>
              </h3>
              <ul>
                {dayEntries.map((vd, index) => {
                  const weekday = vd.date.toLocaleDateString("sv-SE", {
                    weekday: "long",
                  });
                  const time = vd.date.toLocaleTimeString("sv-SE", {
                    hour: "2-digit",
                    minute: "2-digit",
                  });

                  return (
                    <li key={index}>
                      {weekday}, {time} – Avgift: {vd.fee} SEK
                    </li>
                  );
                })}
              </ul>
            </div>
          );
        })}
      </div>
        <Link to={`/users/${encodeURIComponent(name)}`}>← Tillbaka till användaren</Link>   
    </div>
  );
}

export default VehicleDetails;