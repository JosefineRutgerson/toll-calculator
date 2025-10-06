export default function UserImage({ img }) {
  return (
    <div className="user-image">
      {img ? <img src={img} alt="Bild på användare" /> : <p>Ingen användarbild tillgänglig.</p>}
    </div>
  );
}