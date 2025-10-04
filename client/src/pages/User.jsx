import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import UserImage from "../components/UserImage";
import { userImages } from "../components/UserImages";

function User() {
  const { name } = useParams();
  const decodedName = decodeURIComponent(name);

  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const image = userImages[name] || userImages.default;

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await fetch(`http://localhost:5048/api/users/byname/${encodeURIComponent(decodedName)}`);
        if (!res.ok) throw new Error("User not found");
        const data = await res.json();
        setUser(data);
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
      <p className="cursive fee">{user.currentMonthlyFee} SEK</p>
      <h2>Fordon</h2>
      <ul>
        {user.vehicles.map(v => (
          <li key={v.registrationNumber}>
            <Link className="cursive"
              to={`/user/${encodeURIComponent(user.name)}/vehicle/${encodeURIComponent(v.registrationNumber)}`}>
              {v.registrationNumber}: {v.currentMonthlyFee} SEK
            </Link>
          </li>
        ))}
      </ul>
      <p className="cursive">Klicka på registreringsnumret för att se alla taxerade datum.</p>
    </div>
  );
}

export default User;