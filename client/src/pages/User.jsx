import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import UserImage from "../components/UserImage";
import { userImages } from "../components/UserImages";


function User() {
  const { name } = useParams();
  const decodedName = decodeURIComponent(name); // handle spaces, åäö, etc.

  const [user, setUser] = useState(null);
  const [totalFee, setTotalFee] = useState(0);
  const [vehicleFees, setVehicleFees] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const image = userImages[name] || userImages.default;

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch user by name
        const userRes = await fetch(`http://localhost:5048/api/users/byname/${encodeURIComponent(decodedName)}`);
        if (!userRes.ok) throw new Error("User not found");
        const userData = await userRes.json();
        setUser(userData);

        // Fetch total fee
        const feeRes = await fetch(`http://localhost:5048/api/tollcalculator/calculateWholeFee/${encodeURIComponent(decodedName)}`);
        if (!feeRes.ok) throw new Error("Fee not found");
        const feeData = await feeRes.json();
        setTotalFee(feeData);

        // Fetch fees per vehicle
        const fees = {};
        await Promise.all(userData.vehicles.map(async (v) => {
          const res = await fetch(`http://localhost:5048/api/tollcalculator/calculateWholeFeePerCar/${encodeURIComponent(decodedName)}/${encodeURIComponent(v.registrationNumber)}`);
          if (!res.ok) throw new Error(`Fee not found for vehicle ${v.registrationNumber}`);
          const fee = await res.json();
          fees[v.registrationNumber] = fee;
        }));
        setVehicleFees(fees);

      } catch (err) {
        setError(err.message);
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [decodedName]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!user) return <p>No user data</p>;

  return (
    <div>
      <UserImage img={image} />
      <h1>{user.name}</h1>      
      <h2>Aktuell månadsavgift</h2>
      <p className="cursive fee">{totalFee} SEK</p>
      <h2>Fordon</h2>
      <ul>
        {user.vehicles.map(v => (
          <li key={v.registrationNumber}>
            {v.registrationNumber} - Totalt: {vehicleFees[v.registrationNumber] ?? 0} SEK.
          </li>
        ))}
      </ul>
      <p className="cursive">Klicka på registreringsnummret för att se alla taxerade datum.</p>
    </div>
  );
}

export default User;