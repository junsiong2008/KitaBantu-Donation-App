using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KitaBantu.Models
{
    
    class CategoryData
    {
        public ObservableCollection<Category> categories { get; set; }
        public CategoryData()
        {
            categories = new ObservableCollection<Category>();

            categories.Add(new Category { categoryName = "Animals", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fanimal.jpg?alt=media&token=6cff18b6-c6c8-4d32-a237-94dac2373ca5" });
            categories.Add(new Category { categoryName = "B40", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fb40.jpg?alt=media&token=eb880da5-6916-4da8-8c08-e052ce76a59d" });
            categories.Add(new Category { categoryName = "Baby Care", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fbaby_care.jpg?alt=media&token=57f2a46a-4f27-4091-af1b-a574cbd37c61" });
            categories.Add(new Category { categoryName = "Cash", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fcash.jpg?alt=media&token=20b7d405-a3c2-4293-aa7a-9ee5e544217e" });
            categories.Add(new Category { categoryName = "Food", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Ffood.jpg?alt=media&token=35f7c840-390c-413e-9a12-a661ebdc92be" });
            categories.Add(new Category { categoryName = "Frontliners", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Ffrontliners.jpg?alt=media&token=e61c4d78-3e55-4541-b054-26c83cfe645c" });
            categories.Add(new Category { categoryName = "Orang Asli", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Findigenous.jpg?alt=media&token=a097152e-472d-4e60-b6f9-ba60755d7ad1" });
            categories.Add(new Category { categoryName = "Personal Items", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fpersonal.jpg?alt=media&token=cc448cb3-5a90-43df-b075-271c1c606b38" });
            categories.Add(new Category { categoryName = "PPE", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fppe.jpg?alt=media&token=e7fdbc45-60dd-414f-80ca-2adf1f3d7074" });
            categories.Add(new Category { categoryName = "Refugees and Migrants", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Frefugees.jpg?alt=media&token=bbb2c2f9-1e3d-4811-b595-e98c94db088b" });
            categories.Add(new Category { categoryName = "Students", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fstudent.jpg?alt=media&token=1d62f16b-6bef-42c7-bd65-b289dc4f67f8" });
            categories.Add(new Category { categoryName = "Vulnerable Community", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fvulnerable.jpg?alt=media&token=a21e86c5-4053-437f-8baf-2bf9cc5cbbd2" });
            categories.Add(new Category { categoryName = "Women and Children", iconUrl = "https://firebasestorage.googleapis.com/v0/b/kitabantu-de6a8.appspot.com/o/CategoryImages%2Fwomen.jpg?alt=media&token=049b934a-e53f-45f1-b096-ab5364da7b34" });
            
        }
    }
}
