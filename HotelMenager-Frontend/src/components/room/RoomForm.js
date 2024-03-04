import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { updateDashboardRoom, getDashboardRoomUpdateById } from '../../services/api_dashboard_room';
import RoomType from '../filter/FilterRoomTypes';

export default function RoomForm() {
  const { id } = useParams();
  const [mode, setMode] = useState('view');
  const [roomData, setRoomData] = useState({
    imageUrl: '',
    number: '',
    bedCount: 0,
    price: 0,
    typeId: '',
    isActive: true
  });
  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (id && !submitted) {
      getDashboardRoomUpdateById(id)
        .then(response => {
          setRoomData(response.data);
          setMode('view');
        })
        .catch(error => console.error("Error fetching room details:", error));
    }
  }, [id, submitted]);

  const handleFormSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (mode === 'view') {
      setMode('edit');
    } else {
      updateDashboardRoom(id, roomData)
        .then(() => handleSuccess())
        .catch(error => console.error("Error updating room:", error));
    }
  };

  const handleSuccess = () => {
    console.log('Room updated successfully!');
  };

  const handleRoomTypeChange = (selectedRoomType) => {
    setRoomData({ 
      ...roomData, 
      typeId: selectedRoomType,
    });
  };
  

  console.log(roomData);
  return (
    <div>
      <br/>
      <form id="roomForm" onSubmit={handleFormSubmit}>
        <label htmlFor="imageUrl">Image url: </label><br />
        <input type="text" id="imageUrl" name="imageUrl" value={roomData.imageUrl}  disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, imageUrl: e.target.value })} /><br /><br />

        <label htmlFor="roomNumber">Room number: </label><br />
        <input type="text" id="roomNumber" name="roomNumber" value={roomData.number} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, number: e.target.value })} /><br /><br />

        <label className="NumberOfBedsRow">
            <span>Number of beds:</span><br/>
            <input type="text" value={roomData.bedCount} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, bedCount: e.target.value })} /><br/><br/>
        </label>

        <label htmlFor="price">Price for room: </label><br />
        <input type="number" id="price" name="price" value={roomData.price} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, price: e.target.value })} /><br /><br />

        <RoomType onChangeHandle={handleRoomTypeChange} isDisabled={mode === 'view'} />


        <label>
          Is Active:
          <input 
            type="text" 
            name="isActive" 
            value={roomData.isActive}
            checked={roomData.isActive} 
            disabled={mode === 'view'} 
            onChange={(e) => setRoomData({ ...roomData, isActive: e.target.value })} 
            />
        </label>

        <br />
        <button type="submit" >{mode === 'view' ? 'Edit' : 'Finish'}</button>
      </form> 
    </div>
  );
}
