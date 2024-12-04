# Unity-Initial

## Project Setup

### Quy tắc viết commit message

| Tiền tố | Mô tả                           |
|---------|---------------------------------|
| Add     | Thêm tài nguyên/ chức năng      |
| Delete  | Xóa  tài nguyên/ chức năng      |
| Modify  | Chỉnh sửa tài nguyên/ chức năng |
| Fix     | Fix bug thành công              |

```
Ví dụ: 
- Add: player movement script
- Fix: Cannot open CanvasShop 
```

### Workflow
Luồng làm việc khi nhận task:
- B1: Tạo nhánh mới đặt tên theo task: <tên người làm>\<tên task>
- B2: Khi làm xong, merge nhánh **main** vào task đang làm, fix các conflicts nếu có
- B3: Đảm bảo sau khi merge thì game chạy bình thường và push lên Github
- B4: Báo lại trên nhóm khi xong 3 bước đầu 
- B5: Leader thực hiện merge các nhánh con vào nhánh main
- B6: Leader xử lý conflict nếu có
- B7: Leader đảm bảo nhánh **main** chạy thành công và xóa các nhánh con vừa hoàn thành
- B8: Leader thông báo sau khi hoàn thành B5, 6, 7

Lưu ý: 
- Khi merge về nhánh của mình, nếu có conflict về Scene -> ưu tiên chọn scene từ nhánh main
- Khi làm việc hạn chế chỉnh sửa trên Scene (chỉ nên chỉnh sửa trên prefab), nếu bắt buộc phải sửa Scene -> kéo phần mình làm việc thành Prefab. Phải đảm bảo khi người khác kéo prefab này ngược trở lại lên Scene, game phải chạy bình thường
- Khi commit, những file nào mà đạt đủ 2 điều kiện này thì discard changes (tránh làm loãng commit và lỗi phát sinh):
  - Ấn vào thì không thấy hiện gì ở phần thay đổi
  - Mình không trực tiếp động vào những file này


## Quy tắc code
**Code thì phải comment cho người khác đọc hiểu**

**Variables**:  
- Đặt tên biến
  - private: _name 
  - protected and public variables: Name 
  - Ví dụ:

      ``private string _playerId;``  
      ``public int Health;``

- Không sử dụng **magic values**
  - File DefineValue.cs là 1 ví dụ về việc tránh dùng **magic values**

**Function**:
- Mỗi function đều cần có ghi chú, sử dụng thẻ <summary></summary> (gõ 3 dấu "/" phía trên 1 function là sẽ hiện)