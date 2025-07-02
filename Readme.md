# Metabase Embedded App

- Bu projede self-hosted Metabase üzerinde oluşturulan dashboard'ları bir web uygulamasına gömme işlemi gerçekleştirilmiştir.

# Metabase platformunun kurulması

- Docker kullanılarak metabase imajı yüklenmiştir.
- Ardından bu imajdan bir container oluşturulmuştur.
- Metabase localhost:3000 üzerinde çalışmaktadır.

# Web Uygulamasının oluşturulması

- Asp .NET Core MVC şablonuyla bir proje oluşturulmuştur.
- Dashboard'ın gömüleceği ekran için birer controller ve view oluşturulmuştur.

# JsonWebToken oluşturulması

1. JWT için secret key Metabase üzerinden üretildi.
2. Gerekli paketler kurularak JWT oluşturan bir servis tasarlandı.
3. Kullanıcıyı yetkilendiren

# Gömme (Embedding) işleminin tamamlanması

- Dashboard, `<iframe src="{url}/embed/dashboard/{jwtoken}"></iframe>`
  şeklinde ayarlanarak sayfaya gömülmüştür.
